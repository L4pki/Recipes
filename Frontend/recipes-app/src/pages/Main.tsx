import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './styles/Favorite.css'; // Добавьте стили, если нужно
import { Recipe, RecipeStatus, Tag } from '../types/recipe';
import { checkStatusLikeStarRecipe, GetMostLikedRecipes, likeRecipe, starRecipe, SearchRecipes, GetPopularTagList } from '../api/recipeService';
import RecipeCard from '../components/RecipeCard/RecipeCard';
import PopularTags from '../components/forms/TagForm';

const Main: React.FC = () => {
    const navigate = useNavigate();
    const [recipes, setRecipes] = useState<Recipe[]>([]);
    const [recipeStatuses, setRecipeStatuses] = useState<{ [key: number]: RecipeStatus | undefined }>({});
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [searchString, setSearchString] = useState<string>('');
    const [popularTags, setPopularTags] = useState<Tag[]>([]);

    const isAuthenticated = !!localStorage.getItem('token');

    const handleAddRecipe = () => {
        if (isAuthenticated) {
            navigate('/RecipeCreate');
        } else {
            alert('Необходимо авторизоваться');
        }
    };

    const handleAuthorization = () => {
        navigate('/authorization');
    };

    const fetchMostLikedRecipes = async () => {
        setLoading(true);
        setError(null);
        try {
            const response = await GetMostLikedRecipes();
            if (response) {
                const recipesArray = response.recipes.$values || [];
                setRecipes(recipesArray);
                const statuses = await Promise.all(recipesArray.map(recipe => checkStatusLikeStarRecipe(recipe.id)));
                const statusesMap = recipesArray.reduce((acc, recipe, index) => {
                    acc[recipe.id] = statuses[index]; 
                    return acc;
                }, {} as { [key: number]: RecipeStatus | undefined });
                setRecipeStatuses(statusesMap);
            } else {
                setError('Не удалось получить избранные рецепты');
            }
        } catch (err) {
            setError('Ошибка при загрузке рецептов');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchMostLikedRecipes();
        fetchPopularTags();
    }, []);

    const handleLike = async (recipeId: number) => {
        try {
            const currentStatus = recipeStatuses[recipeId]?.recipeLiked;
            await likeRecipe(recipeId);
            setRecipes(prevRecipes =>
                prevRecipes.map(recipe => recipe.id === recipeId ? { 
                    ...recipe, 
                    usersLikesCount: currentStatus ? (recipe.usersLikesCount || 0) - 1 : (recipe.usersLikesCount || 0) + 1 
                } : recipe)
            );
            setRecipeStatuses(prevStatuses => ({
                ...prevStatuses,
                [recipeId]: { ...prevStatuses[recipeId], recipeLiked: !currentStatus }
            }));
        } catch {
            setError('Ошибка при установке лайка');
        }
    };

    const handleStar = async (recipeId: number) => {
        try {
            const currentStatus = recipeStatuses[recipeId]?.recipeStarred;
            await starRecipe(recipeId);
            const updatedRecipeStatus = await checkStatusLikeStarRecipe(recipeId);
            if (updatedRecipeStatus) {
                setRecipes(prevRecipes =>
                    prevRecipes.map(recipe => recipe.id === recipeId ? { 
                        ...recipe, 
                        usersStarsCount: currentStatus ? (recipe.usersStarsCount || 0) - 1 : (recipe.usersStarsCount || 0) + 1 
                    } : recipe)
                );
                setRecipeStatuses(prevStatuses => ({
                    ...prevStatuses,
                    [recipeId]: { ...prevStatuses[recipeId], recipeStarred: updatedRecipeStatus.recipeStarred }
                }));
            }
        } catch {
            setError('Ошибка при установке звезды');
        }
    };

    const handleSearch = async (searchTerm: string) => {
        setLoading(true);
        setError(null);
        try {
            const response = await SearchRecipes(searchTerm);
            if (response && response.recipes) {
                const recipesArray = response.recipes.$values || [];
                setRecipes(recipesArray);
                const statuses = await Promise.all(recipesArray.map(recipe => checkStatusLikeStarRecipe(recipe.id)));
                const statusesMap = recipesArray.reduce((acc, recipe, index) => {
                    acc[recipe.id] = statuses[index]; 
                    return acc;
                }, {} as { [key: number]: RecipeStatus | undefined });
                setRecipeStatuses(statusesMap);
            } else {
                setError('Не удалось найти рецепты по вашему запросу');
            }
        } catch (err) {
            setError('Ошибка при поиске рецептов');
        } finally {
            setLoading(false);
        }
    };

    const handleTagClick = (tag: string) => {
        setSearchString(tag); // Устанавливаем строку поиска на выбранный тег
        handleSearch(tag); // Передаем новый тег в функцию поиска
    };

    const fetchPopularTags = async () => {
        try {
            const response = await GetPopularTagList();
            if (response && response.tags && Array.isArray(response.tags.$values)) {
                setPopularTags(response.tags.$values.map((tag: { id: number; name: string }) => ({
                    id: tag.id,
                    name: tag.name
                })));
            } else {
                setError('Не удалось загрузить теги');
            }
        } catch (err) {
            setError('Ошибка при загрузке тегов');
        }
    };

    return (
        <div>
            <h1>Главная</h1>
            <input
                type="text"
                value={searchString}
                onChange={(e) => setSearchString(e.target.value)}
                placeholder="Поиск рецептов..."
            />
            <button onClick={() => handleSearch(searchString)}>Поиск</button>
            <button onClick={handleAddRecipe}>
                Добавить рецепт
            </button>
            {!isAuthenticated && (
                <button onClick={handleAuthorization}>
                    Авторизация
                </button>
            )}
            {!isAuthenticated && <p>Вы не авторизованы. Пожалуйста, авторизуйтесь для добавления рецептов.</p>}

            <PopularTags tags={popularTags} onTagClick={handleTagClick} />

            {loading && <p>Загрузка рецептов...</p>}
            {error && <p className="error">{error}</p>}
            {recipes.length > 0 ? (
                <div>
                    <h2>Рецепты дня</h2>
                    <ul>
                        {recipes.map(recipe => (
                            <RecipeCard
                                key={recipe.id}
                                recipe={recipe}
                                recipeStatus={recipeStatuses[recipe.id]}
                                onLike={handleLike}
                                onStar={handleStar}
                            />
                        ))}
                    </ul>
                </div>
            ) : (
                <p>Нет доступных рецептов.</p>
            )}
        </div>
    );
};

export default Main;
