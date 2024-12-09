import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import {
    GetTagAllList,
    CreateRecipeApi,
} from "../api/recipeService";
import {
    UpdateIngredient,
    UpdateRecipe,
    UpdateStep,
    UpdateTag,
} from "../types/recipe";
import "./styles/Detail.css";

const RecipeCreate: React.FC = () => {
    const navigate = useNavigate();
    const [tags, setTags] = useState<UpdateTag[]>([]);
    const [selectedTag, setSelectedTag] = useState<UpdateTag | null>(null);
    const [formData, setFormData] = useState<UpdateRecipe>({
        idRecipe: 0,
        name: "",
        shortDescription: "",
        photoUrl: "",
        timeCosts: "0:0:0",
        numberOfPersons: 0,
        ingridients: [],
        steps: [],
        tags: [],
    });
    
    useEffect(() => {
        const fetchTags = async () => {
            try {
                const response = await GetTagAllList();
                console.log("Полученные теги:", response);
                const tagsList: { name: string }[] = response.$values;
                setTags(tagsList);
            } catch (error) {
                console.error("Ошибка при получении тегов:", error);
            }
        };

        fetchTags();
    },[]);

    const handleChange = (
        e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
    ) => {
        const { name, value } = e.target;
        setFormData((prevData) => ({ ...prevData, [name]: value }));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        console.log(formData);
        const createRecipeResponse = await CreateRecipeApi(formData);
        if (createRecipeResponse) {
            navigate(`/profile`);
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
        const selectedTagObject = tags.find(
            (tag) => tag.name === selectedValue
        );
        if (selectedTagObject) {
            setSelectedTag(selectedTagObject);
        } else {
            setSelectedTag(null);
        }
    };

    const handleAddTag = () => {
        if (selectedTag) {
            setFormData((prevData) => ({
                ...prevData,
                tags: [...(prevData.tags || []), selectedTag],
            }));
            setSelectedTag(null);
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

    return (
        <div className="favorite-recipes">
            <h2 className="recipe-name">Создать рецепт: {formData.name}</h2>
            <form onSubmit={handleSubmit}>
                <img
                    className="recipe-image"
                    src={formData.photoUrl}
                    alt={formData.name}
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
                    {formData.tags.map(
                        (
                            tag,
                            index 
                        ) => (
                            <li key={index}>
                                {tag.name}
                                <button
                                    type="button"
                                    onClick={() => handleRemoveTag(index)}
                                >
                                    Удалить
                                </button>
                            </li>
                        )
                    )}
                </ul>

                <button type="submit">Сохранить рецепт</button>
                <button type="button" onClick={() => navigate(-1)}>
                    Назад
                </button>
            </form>
        </div>
    );
};

export default RecipeCreate;
