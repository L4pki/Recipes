import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { GetTagAllList, CreateRecipeApi } from "../api/recipeService";
import {
    UpdateIngredient,
    UpdateRecipe,
    UpdateStep,
    UpdateTag,
} from "../types/recipe";
import "./styles/RecipeCreate.css";
import backspace from "../assets/images/backspace.png";
import close from "../assets/images/Close.png";
import download from "../assets/images/download.png";
import plus from "../assets/images/plus.png";

const RecipeCreate: React.FC = () => {
    const navigate = useNavigate();
    const [tags, setTags] = useState<UpdateTag[]>([]);
    const [formData, setFormData] = useState<UpdateRecipe>({
        idRecipe: 0,
        name: "",
        shortDescription: "",
        photoUrl: "",
        timeCosts: "0:0:0",
        numberOfPersons: 0,
        ingridients: [{ title: "", description: "" }],
        steps: [],
        tags: [],
    });
    const [imageFile, setImageFile] = useState<File | null>(null);
    const [inputValue, setInputValue] = useState("");
    const [suggestions, setSuggestions] = useState<UpdateTag[]>([]);
    const [isFocused, setIsFocused] = useState(false);

    useEffect(() => {
        const fetchTags = async () => {
            try {
                const response = await GetTagAllList();
                console.log("Полученные теги:", response);
                const tagsList: { name: string }[] = response;
                setTags(tagsList);
            } catch (error) {
                console.error("Ошибка при получении тегов:", error);
            }
        };

        fetchTags();
    }, []);

    const handleChange = (
        e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
    ) => {
        const { name, value } = e.target;
        setFormData((prevData) => ({ ...prevData, [name]: value }));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        console.log(formData);
        if (imageFile) {
            const createRecipeResponse = await CreateRecipeApi(
                formData,
                imageFile
            );
            if (createRecipeResponse) {
                navigate(`/profile`);
            }
        } else {
            console.error("Необходимо выбрать изображение.");
        }
    };

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { value } = e.target;
        setInputValue(value);
        const filteredSuggestions = tags.filter((tag) =>
            tag.name.toLowerCase().includes(value.toLowerCase())
        );
        setSuggestions(filteredSuggestions);
    };

    const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0];
        if (file) {
            setImageFile(file);
            setFormData((prevData) => ({
                ...prevData,
                photoUrl: URL.createObjectURL(file),
            }));
        }
    };

    const handleMinutesChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        const numericValue = Number(value);
        if (isNaN(numericValue) || numericValue < 0) {
            return;
        }
        const hours = Math.floor(numericValue / 60);
        const minutes = numericValue % 60;
        const formattedTime = `${hours.toString().padStart(2, "0")}:${minutes
            .toString()
            .padStart(2, "0")}:00`;

        setFormData((prevData) => ({
            ...prevData,
            timeCosts: formattedTime,
        }));
    };

    const handleChangeIngredient = (
        e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
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

    const addTag = (tag: UpdateTag) => {
        if (!formData.tags.includes(tag)) {
            setFormData((prevState) => ({
                ...prevState,
                tags: [...prevState.tags, tag],
            }));
            setInputValue("");
            setSuggestions([]);
        }
    };

    const removeTag = (tagToRemove: UpdateTag) => {
        setFormData((prevState) => ({
            ...prevState,
            tags: prevState.tags.filter((tag) => tag !== tagToRemove),
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
        <div className="create-recipe-page">
            <button
                className="backspace-button"
                onClick={() => navigate("/main")}
            >
                <img src={backspace} alt="Назад" />
                <p className="backspace-text">Назад</p>
            </button>
            <div className="title-button-block">
                <h2 className="create-recipe-title">Добавить новый рецепт</h2>
                <button className="create-recipe-button" onClick={handleSubmit}>Опубликовать</button>
            </div>

            <form className="create-recipe-form" onSubmit={handleSubmit}>
                <div className="create-recipe-first-block">
                    <div
                        className="create-recipe-image-block"
                        onClick={() =>
                            document.getElementById("imageInput")?.click()
                        }
                    >
                        {formData.photoUrl.length > 0 ? (
                            <img
                                className="create-recipe-image"
                                src={formData.photoUrl}
                                alt={formData.name}
                            />
                        ) : (
                            <div className="create-recipe-image">
                                <div className="import-image-rectangle">
                                    <img src={download} alt="download" />
                                    <div className="import-image-text">
                                        <p>Загрузите фото</p>
                                        <p>готового блюда</p>
                                    </div>
                                </div>
                            </div>
                        )}
                    </div>
                    <input
                        id="imageInput"
                        type="file"
                        accept="image/*"
                        onChange={handleImageChange}
                        style={{ display: "none" }}
                    />
                    <ul className="create-recipe-info-block">
                        <li>
                            <input
                                className="recipe-info-name"
                                type="text"
                                name="name"
                                placeholder="Название рецепта"
                                value={formData.name}
                                onChange={handleChange}
                                required
                            />
                        </li>
                        <li>
                            <textarea
                                className="recipe-info-about"
                                name="shortDescription"
                                placeholder="Краткое описание рецепта (150 символов)"
                                value={formData.shortDescription}
                                onChange={handleChange}
                                required
                            />
                        </li>
                        <li>
                            <div className="recipe-tags-rectangle">
                                <ul>
                                    {formData.tags.map((tag, index) => (
                                        <li key={index} className="tag">
                                            {tag.name}
                                            <button
                                                type="button"
                                                onClick={() => removeTag(tag)}
                                            >
                                                x
                                            </button>
                                        </li>
                                    ))}
                                </ul>
                                <input
                                    type="text"
                                    value={inputValue}
                                    onChange={handleInputChange}
                                    placeholder="Добавьте тег"
                                    onFocus={() => setIsFocused(true)}
                                    onBlur={() => setIsFocused(false)}
                                />
                            </div>
                            {isFocused && suggestions.length > 0 && (
                                <ul className="suggestions">
                                    {suggestions.map((tag, index) => (
                                        <li
                                            key={index}
                                            onMouseDown={() => addTag(tag)}
                                        >
                                            {tag.name}
                                        </li>
                                    ))}
                                </ul>
                            )}
                        </li>
                        <li className="recipe-info-time-person">
                            <div className="recipe-info-time">
                                <input
                                    className="recipe-time"
                                    name="timeCosts"
                                    type="number"
                                    onChange={handleMinutesChange}
                                    placeholder="Время готовки"
                                />
                                <p>Минут</p>
                            </div>
                            <div className="recipe-info-time">
                                <input
                                    className="recipe-time"
                                    name="numberOfPersons"
                                    type="number"
                                    value={formData.numberOfPersons}
                                    onChange={handleChange}
                                    placeholder="Порций в блюде"
                                />
                                <p>Персон</p>
                            </div>
                        </li>
                    </ul>
                </div>
                <div className="create-recipe-second-block">
                    <div className="create-recipe-ingridients">
                        <h3 className="ingridients-title">Ингредиенты</h3>
                        <div className="ingridient-list">
                            {formData.ingridients.map((ingredient, index) => (
                                <div key={index} className="ingredient-item">
                                    <button
                                        type="button"
                                        className="remove-ingredient-button"
                                        onClick={() => removeIngredient(index)}
                                    >
                                        <img src={close} alt="Закрыть" />
                                    </button>
                                    <input
                                        className="recipe-ingredient-name"
                                        type="text"
                                        placeholder="Загаловок для ингридиентов"
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
                                    <textarea
                                        className="recipe-ingredient-description"
                                        placeholder="Список продуктов для категории"
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
                                </div>
                            ))}
                        </div>
                        <button
                            className="button-add-new-ingr-step"
                            onClick={addIngredient}
                        >
                            <img src={plus} alt="plus" />
                            Добавить загаловок
                        </button>
                    </div>
                    <div className="create-recipe-steps">
                        <div className="step-list">
                            {formData.steps.map((step, index) => (
                                <div key={index} className="step-item">
                                    <div className="step-title">
                                        <p className="step-number">
                                            Шаг {index+1}
                                        </p>
                                        <button
                                            type="button"
                                            className="remove-step-button"
                                            onClick={() => removeStep(index)}
                                        >
                                            <img src={close} alt="Закрыть" />
                                        </button>
                                    </div>
                                    <textarea
                                        className="recipe-step-description"
                                        placeholder="Описание шага"
                                        value={step.description}
                                        onChange={(e) =>
                                            handleChangeStep(e, index)
                                        }
                                        required
                                    />
                                </div>
                            ))}
                        </div>
                        <button
                            className="button-add-new-ingr-step"
                            onClick={addStep}
                        >
                            <img src={plus} alt="plus" />
                            Добавить шаг
                        </button>
                    </div>
                </div>
            </form>
        </div>
    );
};

export default RecipeCreate;
