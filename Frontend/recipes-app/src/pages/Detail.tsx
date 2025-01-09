import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import {
    checkStatusLikeStarRecipe,
    getRecipeDetail,
    likeRecipe,
    starRecipe,
    DeleteRecipe
} from "../api/recipeService";
import { infoUser } from "../api/userService";
import { Recipe, RecipeDetail, RecipeStatus } from "../types/recipe";
import "./styles/Detail.css";
import { RecipeCard } from "../components/RecipeCard/RecipeCard";
import Backspace from "../components/forms/Backspace";
import edit from "../assets/images/edit-white.png";
import deleteImg from "../assets/images/delete.png";

const Detail: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const [recipe, setRecipe] = useState<RecipeDetail | undefined>(undefined);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [recipeStatuses, setRecipeStatuses] = useState<
        RecipeStatus | undefined
    >({});
    const [isPersonalRecipe, setIsPersonalRecipe] = useState<boolean>(false);

    useEffect(() => {
        const fetchRecipeDetail = async () => {
            try {
                const recipeDetail = await getRecipeDetail(Number(id));
                setRecipe(recipeDetail.recipe);
                const statuses = await checkStatusLikeStarRecipe(Number(id));
                setRecipeStatuses(statuses);
            } catch (err) {
                setError("Ошибка при загрузке данных о рецепте");
            } finally {
                setLoading(false);
            }
        };

        const checkIfPersonalRecipe = async () => {
            try {
                const userInfo = await infoUser();
                const personalRecipes = userInfo.personalRecipes;
                const isPersonal = personalRecipes.some(
                    (personalRecipe: Recipe) => personalRecipe.id === Number(id)
                );
                setIsPersonalRecipe(isPersonal);
            } catch (err) {
                setError("Ошибка при загрузке данных о пользователе");
            }
        };

        fetchRecipeDetail();
        checkIfPersonalRecipe();
    }, [id]);

    const handleLike = async (recipeId: number) => {
        try {
            const currentStatus = recipeStatuses?.recipeLiked;
            await likeRecipe(recipeId);
            setRecipe((prevRecipe) => ({
                ...prevRecipe!,
                usersLikesCount: currentStatus
                    ? (prevRecipe!.usersLikesCount || 0) - 1
                    : (prevRecipe!.usersLikesCount || 0) + 1,
            }));
            setRecipeStatuses((prevStatuses) => ({
                ...prevStatuses,
                recipeLiked: !currentStatus,
            }));
        } catch {
            setError("Ошибка при установке лайка");
        }
    };

    const handleStar = async (recipeId: number) => {
        try {
            const currentStatus = recipeStatuses?.recipeStarred;
            await starRecipe(recipeId);
            const updatedRecipeStatus = await checkStatusLikeStarRecipe(
                recipeId
            );
            if (updatedRecipeStatus) {
                setRecipe((prevRecipe) => ({
                    ...prevRecipe!,
                    usersStarsCount: currentStatus
                        ? (prevRecipe!.usersStarsCount || 0) - 1
                        : (prevRecipe!.usersStarsCount || 0) + 1,
                }));
                setRecipeStatuses((prevStatuses) => ({
                    ...prevStatuses,
                    recipeStarred: updatedRecipeStatus.recipeStarred,
                }));
            }
        } catch {
            setError("Ошибка при установке звезды");
        }
    };

    const handleDelete = async () => {
        try {
            if (recipe && recipe.id) {
                await DeleteRecipe(recipe.id);
                navigate('/profile');
            } else {
                setError("Рецепт не найден");
            }
        } catch {
            setError("Ошибка при удалении рецепта");
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
        <div className="detail-page">
            <Backspace />
            <div className="detail-header">
                <h3 className="detail-title">{recipe.name}</h3>
                <div className="detail-header-buttons">
                    {isPersonalRecipe && (
                        <>
                            <button 
                                className="delete-recipe-button"
                                onClick={handleDelete}
                            >
                                <img src={deleteImg} alt="Корзина" />
                            </button>
                            <button
                                className="edit-recipe-button"
                                onClick={() => navigate(`/RecipeEdit/${id}`)}
                            >
                                <img src={edit} alt="Редактировать" />
                                Редактировать
                            </button>
                        </>
                    )}
                </div>
            </div>
            <RecipeCard
                key={recipe.id}
                recipe={recipe}
                recipeStatus={recipeStatuses}
                onLike={handleLike}
                onStar={handleStar}
            />
            <div className="detail-ingredients-steps">
                <ul className="detail-ingredients">
                    <p className="detail-ingredients-title">Ингридиенты</p>
                    {recipe.ingridientForCooking &&
                    recipe.ingridientForCooking ? (
                        recipe.ingridientForCooking.map((ingredient) => (
                            <li
                                className="detail-ingredients-item"
                                key={ingredient.id}
                            >
                                <p className="detail-ingredients-name">
                                    {ingredient.title}
                                </p>
                                <p className="detail-ingredients-description">
                                    {ingredient.description}
                                </p>
                            </li>
                        ))
                    ) : (
                        <li className="detail-ingredients-item">
                            Нет ингредиентов
                        </li>
                    )}
                </ul>
                <ul className="detail-steps">
                    {recipe.stepOfCooking && recipe.stepOfCooking ? (
                        recipe.stepOfCooking.map((step, index) => (
                            <li className="detail-steps-item" key={index}>
                                <p className="detail-number-step">
                                    Шаг {step.numberOfStep}
                                </p>
                                <p className="detail-step-description">
                                    {step.description}
                                </p>
                            </li>
                        ))
                    ) : (
                        <li className="recipe-item">Нет шагов</li>
                    )}
                    <p className="detail-sentiment">Приятного Аппетита!</p>
                </ul>
            </div>
        </div>
    );
};

export default Detail;
