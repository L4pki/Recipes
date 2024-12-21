import { useEffect, useState } from 'react';
import { getFavoriteRecipes, checkStatusLikeStarRecipe, likeRecipe, starRecipe } from '../api/recipeService';
import { Recipe, RecipeStatus } from '../types/recipe';
import './styles/Favorite.css';
import { RecipeCard } from '../components/RecipeCard/RecipeCard';

const Favorite = () => {
    const [favoriteRecipes, setFavoriteRecipes] = useState<Recipe[]>([]);
    const [recipeStatuses, setRecipeStatuses] = useState<{ [key: number]: RecipeStatus | undefined }>({});
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchFavoriteRecipes = async () => {
            const response = await getFavoriteRecipes();
            if (response) {
                const recipesArray = response.recipes.$values || [];
                setFavoriteRecipes(recipesArray);
                const statuses = await Promise.all(recipesArray.map(recipe => checkStatusLikeStarRecipe(recipe.id)));
                const statusesMap = recipesArray.reduce((acc, recipe, index) => {
                    acc[recipe.id] = statuses[index]; 
                    return acc;
                }, {} as { [key: number]: RecipeStatus | undefined });
                setRecipeStatuses(statusesMap);
            } else {
                setError('Не удалось получить избранные рецепты');
            }
        };

        fetchFavoriteRecipes();
    }, []);

    const handleLike = async (recipeId: number) => {
        try {
            const currentStatus = recipeStatuses[recipeId]?.recipeLiked;
            await likeRecipe(recipeId);
            setFavoriteRecipes(prevRecipes =>
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
                setFavoriteRecipes(prevRecipes =>
                    prevRecipes.map(recipe => recipe.id === recipeId ? { 
                        ...recipe, 
                        usersStarsCount: currentStatus ? recipe.usersStarsCount - 1 : recipe.usersStarsCount + 1 
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

    return (
        <div className="favorite-recipes">
            <h2>Избранные рецепты</h2>
            {error && <p className="error">{error}</p>}
            <ul className="recipes-list">
                {favoriteRecipes.length > 0 ? (
                    favoriteRecipes.map(recipe => (
                        <RecipeCard
                            key={recipe.id}
                            recipe={recipe}
                            recipeStatus={recipeStatuses[recipe.id]}
                            onLike={handleLike}
                            onStar={handleStar}
                        />
                    ))
                ) : (
                    <li>Нет избранных рецептов.</li>
                )}
            </ul>
        </div>
    );
}

export default Favorite;
