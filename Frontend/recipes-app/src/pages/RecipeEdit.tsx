import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import {
    getRecipeDetail,
    UpdateRecipeApi,
    GetTagAllList,
} from "../api/recipeService";
import {
    RecipeDetail,
    UpdateIngredient,
    UpdateRecipe,
    UpdateStep,
    UpdateTag,
} from "../types/recipe";
import "./styles/Detail.css";

const RecipeEdit: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const [recipe, setRecipe] = useState<RecipeDetail | undefined>(undefined);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [formData, setFormData] = useState<UpdateRecipe>({
        idRecipe: Number(id),
        name: "",
        shortDescription: "",
        photoUrl: "",
        timeCosts: "0:0:0",
        numberOfPersons: 1,
        ingridients: [],
        steps: [],
        tags: [],
    });
    const [tags, setTags] = useState<UpdateTag[]>([]); // Теги все
    const [selectedTag, setSelectedTag] = useState<UpdateTag | null>(null); // Изменено на строку
    useEffect(() => {
        const fetchRecipeDetail = async () => {
            try {
                const recipeDetail = await getRecipeDetail(Number(id));
                if (recipeDetail && recipeDetail.recipe) {
                    setRecipe(recipeDetail.recipe);
                    setFormData({
                        idRecipe: Number(id),
                        name: recipeDetail.recipe.name || "",
                        shortDescription:
                            recipeDetail.recipe.shortDescription || "",
                        photoUrl: recipeDetail.recipe.photoUrl || "",
                        timeCosts: recipeDetail.recipe.timeCosts || "0:0:0",
                        numberOfPersons:
                            recipeDetail.recipe.numberOfPersons || 1,
                        ingridients:
                            recipeDetail.recipe.ingridientForCooking?.$values ||
                            [],
                        tags:
                            recipeDetail.recipe.tags?.$values.map(
                                (tag: { name: string }) => tag
                            ) || [],
                        steps: recipeDetail.recipe.stepOfCooking?.$values || [],
                    });
                }
            } catch (err) {
                setError("Ошибка при загрузке данных о рецепте");
            } finally {
                setLoading(false);
            }
        };

        const fetchTags = async () => {
            try {
                const response = await GetTagAllList();
                console.log("Полученные теги:", response); // Лог для проверки

                // Извлечение массива тегов из объекта
                const tagsList: { name: string }[] = response.$values; // Указываем тип для tagsList

                // Преобразование тегов в массив строк
                //const formattedTags: string[] = tagsList.map((tag) => tag.name); // Исправлено на строку

                setTags(tagsList); // Устанавливаем преобразованные теги в состояние
            } catch (error) {
                console.error("Ошибка при получении тегов:", error);
            }
        };

        fetchRecipeDetail();
        fetchTags();
    }, [id]);

    const handleChange = (
        e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
    ) => {
        const { name, value } = e.target;
        setFormData((prevData) => ({ ...prevData, [name]: value }));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (
            Array.isArray(formData.ingridients) &&
            formData.ingridients.length > 0 &&
            Array.isArray(formData.steps) &&
            formData.steps.length > 0 &&
            Array.isArray(formData.tags) &&
            formData.tags.length > 0
        ) {
            console.log(formData);
            const updatedRecipe = await UpdateRecipeApi(Number(id), formData);
            if (updatedRecipe) {
                console.log(formData);
                navigate(`/detail/${id}`);
            }
        } else {
            setError("Необходимо добавить хотя бы один ингредиент и один шаг.");
        }
    };

    const handleTimeChange = (
        e: React.ChangeEvent<HTMLInputElement>,
        type: "hours" | "minutes" | "seconds"
    ) => {
        const value = e.target.value;

        // Преобразуем введенное значение в число
        const numericValue = Number(value);

        // Проверяем, является ли введенное значение числом
        if (isNaN(numericValue)) {
            return; // Если не число, ничего не делаем
        }

        setFormData((prevData) => {
            const timeParts = prevData.timeCosts.split(":");

            // В зависимости от типа поля, устанавливаем новое значение
            if (type === "hours") {
                // Устанавливаем часы, не ограничивая их
                timeParts[0] = Math.max(0, numericValue)
                    .toString()
                    .padStart(2, "0");
            } else if (type === "minutes") {
                // Устанавливаем минуты, не ограничивая их
                timeParts[1] = Math.max(0, numericValue)
                    .toString()
                    .padStart(2, "0");
            } else if (type === "seconds") {
                // Устанавливаем секунды, не ограничивая их
                timeParts[2] = Math.max(0, numericValue)
                    .toString()
                    .padStart(2, "0");
            }

            // Преобразуем значения в формат "00:00:00"
            const formattedTime = timeParts
                .map((part) => part.padStart(2, "0"))
                .join(":");

            return { ...prevData, timeCosts: formattedTime };
        });
    };

    const handleChangeIngredient = (
        e: React.ChangeEvent<HTMLInputElement>,
        index: number,
        field: "title" | "description"
    ) => {
        const { value } = e.target;
        if (formData) {
            const updatedIngredients = formData.ingridients?.map(
                (ingredient, ingredientIndex) => {
                    if (ingredientIndex === index) {
                        return { ...ingredient, [field]: value };
                    }
                    return ingredient;
                }
            );

            setFormData({
                ...formData,
                ingridients: updatedIngredients,
            });
        }
    };

    const handleChangeStep = (
        e: React.ChangeEvent<HTMLTextAreaElement>,
        index: number
    ) => {
        const { value } = e.target;
        if (formData) {
            const updatedSteps = formData.steps?.map((step, stepIndex) => {
                if (stepIndex === index) {
                    return { ...step, description: value };
                }
                return step;
            });

            setFormData({ ...formData, steps: updatedSteps });
        }
    };

    const addIngredient = () => {
        const newIngredient: UpdateIngredient = { title: "", description: "" };
        setFormData((prevData) => ({
            ...prevData,
            ingridients: [...(prevData?.ingridients || []), newIngredient],
        }));
    };

    const removeIngredient = (index: number) => {
        if (formData) {
            const updatedIngredients = formData.ingridients?.filter(
                (_, ingredientIndex) => ingredientIndex !== index
            );
            setFormData({
                ...formData,
                ingridients: updatedIngredients,
            });
        }
    };

    const handleTagChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const selectedValue = e.target.value;

        // Найдите тег по выбранному значению
        const selectedTagObject = tags.find(
            (tag) => tag.name === selectedValue
        );

        if (selectedTagObject) {
            // Если тег найден, обновите состояние
            setSelectedTag(selectedTagObject); // Сохраняем только один выбранный тег
        } else {
            setSelectedTag(null); // Если ничего не выбрано, сбросьте состояние
        }
    };

    const handleAddTag = () => {
        if (selectedTag) {
            setFormData((prevData) => ({
                ...prevData,
                tags: [...prevData.tags, { name: selectedTag.name }], // Добавляем тег в массив Tags
            }));
            setSelectedTag(null); // Сбросить выбранный тег
        }
    };

    const handleRemoveTag = (index: number) => {
        const updatedTags = formData.tags.filter(
            (_, tagIndex) => tagIndex !== index
        );
        setFormData((prevData) => ({
            ...prevData,
            tags: updatedTags,
        }));
    };

    const addStep = () => {
        const newStep: UpdateStep = {
            numberOfStep: (formData.steps?.length || 0) + 1,
            description: "",
        };
        setFormData((prevData) => ({
            ...prevData,
            steps: [...(prevData?.steps || []), newStep],
        }));
    };

    const removeStep = (index: number) => {
        if (formData) {
            const updatedSteps = formData.steps?.filter(
                (_, stepIndex) => stepIndex !== index
            );
            setFormData({ ...formData, steps: updatedSteps });
        }
    };

    if (loading) {
        return <div className="error">Загрузка...</div>;
    }

    if (error) {
        return <div className="error">{error}</div>;
    }

    if (!recipe) {
        return <div className="error">Рецепт не найден.</div>;
    }

    return (
        <div className="favorite-recipes">
            <h2 className="recipe-name">Редактировать рецепт: {recipe.name}</h2>
            <form onSubmit={handleSubmit}>
                <img
                    className="recipe-image"
                    src={recipe.photoUrl}
                    alt={recipe.name}
                />
                <div>
                    <label>URL Фото:</label>
                    <input
                        type="text"
                        name="photoUrl"
                        value={formData.photoUrl}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div>
                    <label>Название:</label>
                    <input
                        type="text"
                        name="name"
                        value={formData.name}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div>
                    <label>Краткое описание:</label>
                    <textarea
                        name="shortDescription"
                        value={formData.shortDescription}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div>
                    <label>Время приготовления:</label>
                    <div>
                        <input
                            type="number"
                            placeholder="Часы"
                            value={formData.timeCosts.split(":")[0] || ""}
                            onChange={(e) => handleTimeChange(e, "hours")}
                        />
                        <input
                            type="number"
                            placeholder="Минуты"
                            value={formData.timeCosts.split(":")[1] || ""}
                            onChange={(e) => handleTimeChange(e, "minutes")}
                        />
                        <input
                            type="number"
                            placeholder="Секунды"
                            value={formData.timeCosts.split(":")[2] || ""}
                            onChange={(e) => handleTimeChange(e, "seconds")}
                        />
                    </div>
                </div>
                <div>
                    <label>Количество порций:</label>
                    <input
                        type="number"
                        name="numberOfPersons"
                        value={formData.numberOfPersons}
                        onChange={handleChange}
                        required
                    />
                </div>
                <h3>Ингредиенты:</h3>
                <ul className="recipes-list">
                    {Array.isArray(formData?.ingridients) ? (
                        formData.ingridients.map((ingredient, index) => (
                            <li key={index}>
                                <input
                                    type="text"
                                    placeholder="Название"
                                    value={ingredient.title}
                                    onChange={(e) =>
                                        handleChangeIngredient(
                                            e,
                                            index,
                                            "title"
                                        )
                                    }
                                    required
                                />
                                <input
                                    type="text"
                                    placeholder="Описание"
                                    value={ingredient.description}
                                    onChange={(e) =>
                                        handleChangeIngredient(
                                            e,
                                            index,
                                            "description"
                                        )
                                    }
                                    required
                                />
                                <button
                                    type="button"
                                    onClick={() => removeIngredient(index)}
                                >
                                    Удалить
                                </button>
                            </li>
                        ))
                    ) : (
                        <li>Нет ингредиентов для отображения.</li>
                    )}
                </ul>
                <button type="button" onClick={addIngredient}>
                    Добавить ингредиент
                </button>

                <h3>Шаги приготовления:</h3>
                <ol className="recipes-list">
                    {Array.isArray(formData?.steps) &&
                    formData.steps.length > 0 ? (
                        formData.steps.map((step, index) => (
                            <li key={index}>
                                <span>{step.numberOfStep}</span>
                                <textarea
                                    value={step.description}
                                    onChange={(e) => handleChangeStep(e, index)}
                                    required
                                />
                                <button
                                    type="button"
                                    onClick={() => removeStep(index)}
                                >
                                    Удалить
                                </button>
                            </li>
                        ))
                    ) : (
                        <li>Нет шагов приготовления для отображения.</li>
                    )}
                </ol>

                <button type="button" onClick={addStep}>
                    Добавить шаг
                </button>

                <h3>Теги:</h3>
                <select
                    value={selectedTag ? selectedTag.name : ""}
                    onChange={handleTagChange}
                >
                    <option value="">Выберите тег</option>
                    {Array.isArray(tags) && tags.length > 0 ? (
                        tags.map((tag, index) => (
                            <option key={index} value={tag.name}>
                                {tag.name}
                            </option>
                        ))
                    ) : (
                        <option disabled>Нет доступных тегов</option>
                    )}
                </select>

                <button type="button" onClick={handleAddTag}>
                    Добавить тег
                </button>
                <ul>
                    {formData.tags.map((tag, index) => (
                        <li key={index}>
                            {tag.name}
                            <button
                                type="button"
                                onClick={() => handleRemoveTag(index)}
                            >
                                Удалить
                            </button>
                        </li>
                    ))}
                </ul>

                <button type="submit">Сохранить изменения</button>
                <button type="button" onClick={() => navigate(-1)}>
                    Назад
                </button>
            </form>
        </div>
    );
};

export default RecipeEdit;
