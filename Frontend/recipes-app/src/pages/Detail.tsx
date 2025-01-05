import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getRecipeDetail } from '../api/recipeService';
import { infoUser  } from '../api/userService';
import { Recipe, RecipeDetail } from '../types/recipe';
import './styles/Detail.css';

const Detail: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const [recipe, setRecipe] = useState<RecipeDetail | undefined>(undefined);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [isPersonalRecipe, setIsPersonalRecipe] = useState<boolean>(false);

    useEffect(() => {
        const fetchRecipeDetail = async () => {
            try {
                const recipeDetail = await getRecipeDetail(Number(id));
                setRecipe(recipeDetail.recipe);
            } catch (err) {
                setError('Ошибка при загрузке данных о рецепте');
            } finally {
                setLoading(false);
            }
        };

        const checkIfPersonalRecipe = async () => {
            try {
                const userInfo = await infoUser ();
                const personalRecipes = userInfo.personalRecipes;
                const isPersonal = personalRecipes.some((personalRecipe: Recipe) => personalRecipe.id === Number(id));
                setIsPersonalRecipe(isPersonal);
            } catch (err) {
                setError('Ошибка при загрузке данных о пользователе');
            }
        };

        fetchRecipeDetail();
        checkIfPersonalRecipe();
    }, [id]);

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
            <h2 className="recipe-name">{recipe.name}</h2>
            <img className="recipe-image" src={recipe.photoUrl} alt={recipe.name} />
            <p>{recipe.shortDescription}</p>
            <p>Время приготовления: {recipe.timeCosts || 'Не указано'}</p>
            <p>Количество порций: {recipe.numberOfPersons}</p>
            <h3>Ингредиенты:</h3>
            <ul className="recipes-list">
                {recipe.ingridientForCooking && recipe.ingridientForCooking ? (
                    recipe.ingridientForCooking.map((ingredient) => (
                        <li className="recipe-item" key={ingredient.id}>{ingredient.title}</li>
                    ))
                ) : (
                    <li className="recipe-item">Нет ингредиентов</li>
                )}
            </ul>
            <h3>Шаги приготовления:</h3>
            <ol className="recipes-list">
                {recipe.stepOfCooking && recipe.stepOfCooking ? (
                    recipe.stepOfCooking.map((step, index) => (
                        <li className="recipe-item" key={index}>{step.description}</li>
                    ))
                ) : (
                    <li className="recipe-item">Нет шагов</li>
                )}
            </ol>
            <h3>Теги:</h3>
            <ul className="recipes-list">
                {recipe.tags && recipe.tags ? (
                    recipe.tags.map((tag) => (
                        <li className="recipe-item" key={tag.id}>{tag.name}</li>
                    ))
                ) : (
                    <li className="recipe-item">Нет тегов</li>
                )}
            </ul>
            <p>Количество лайков: {recipe.usersLikesCount}</p>
            <p>Количество звезд: {recipe.usersStarsCount}</p>
            <button onClick={() => navigate(-1)}>Назад</button>
            {isPersonalRecipe && (
                <button onClick={() => navigate(`/RecipeEdit/${id}`)}>Редактировать рецепт</button>
            )}
        </div>
    );
};

export default Detail;
