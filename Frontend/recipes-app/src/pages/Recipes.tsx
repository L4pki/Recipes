import React, { useEffect, useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import "./styles/Recipes.css";
import { Recipe, RecipeStatus, Tag } from "../types/recipe";
import {
    checkStatusLikeStarRecipe,
    GetMostLikedRecipes,
    likeRecipe,
    starRecipe,
    SearchRecipes,
    GetPopularTagList,
} from "../api/recipeService";
import { RecipeCard } from "../components/RecipeCard/RecipeCard";
import PopularTags from "../components/forms/TagForm";
import plus from "../assets/images/plus-white.png";
import tagIcon1 from "../assets/images/tag-icon1.png";
import tagIcon2 from "../assets/images/tag-icon2.png";
import tagIcon3 from "../assets/images/tag-icon3.png";
import tagIcon4 from "../assets/images/tag-icon4.png";

const Recipes: React.FC = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const [recipes, setRecipes] = useState<Recipe[]>([]);
    const [recipeStatuses, setRecipeStatuses] = useState<{
        [key: number]: RecipeStatus | undefined;
    }>({});
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [searchString, setSearchString] = useState<string>("");
    const [popularTags, setPopularTags] = useState<Tag[]>([]);
    const [visibleCount, setVisibleCount] = useState<number>(4);

    const performSearch = async (searchTerm: string) => {
        setLoading(true);
        setError(null);
        try {
            const response = await SearchRecipes(searchTerm);
            if (response && response.recipes) {
                setRecipes(response.recipes || []);
            } else {
                setError("Не удалось найти рецепты по вашему запросу");
            }
        } catch (err) {
            setError("Ошибка при поиске рецептов");
        } finally {
            setLoading(false);
        }
    };

    const isAuthenticated = !!localStorage.getItem("token");

    const handleAddRecipe = () => {
        if (isAuthenticated) {
            navigate("/RecipeCreate");
        } else {
            alert("Необходимо авторизоваться");
        }
    };

    const fetchMostLikedRecipes = async () => {
        setLoading(true);
        setError(null);
        try {
            const response = await GetMostLikedRecipes();
            if (response) {
                const recipesArray = response.recipes || [];
                setRecipes(recipesArray);
                const statuses = await Promise.all(
                    recipesArray.map((recipe) =>
                        checkStatusLikeStarRecipe(recipe.id)
                    )
                );
                const statusesMap = recipesArray.reduce(
                    (acc, recipe, index) => {
                        acc[recipe.id] = statuses[index];
                        return acc;
                    },
                    {} as { [key: number]: RecipeStatus | undefined }
                );
                setRecipeStatuses(statusesMap);
            } else {
                setError("Не удалось получить избранные рецепты");
            }
        } catch (err) {
            setError("Ошибка при загрузке рецептов");
        } finally {
            setLoading(false);
        }
    };

    const handleSearch = async (searchTerm: string) => {
        setLoading(true);
        setError(null);
        try {
            const response = await SearchRecipes(searchTerm);
            if (response && response.recipes) {
                const recipesArray = response.recipes || [];
                setRecipes(recipesArray);
                const statuses = await Promise.all(
                    recipesArray.map((recipe) =>
                        checkStatusLikeStarRecipe(recipe.id)
                    )
                );
                const statusesMap = recipesArray.reduce(
                    (acc, recipe, index) => {
                        acc[recipe.id] = statuses[index];
                        return acc;
                    },
                    {} as { [key: number]: RecipeStatus | undefined }
                );
                setRecipeStatuses(statusesMap);
            } else {
                setError("Не удалось найти рецепты по вашему запросу");
            }
        } catch (err) {
            setError("Ошибка при поиске рецептов");
        } finally {
            setLoading(false);
        }
    };

    const handleTagClick = (tag: string) => {
        setSearchString(tag);
        handleSearch(tag);
    };

    const fetchPopularTags = async () => {
        try {
            const response = await GetPopularTagList();
            if (response && response.tags && Array.isArray(response.tags)) {
                setPopularTags(
                    response.tags.map((tag: { id: number; name: string }) => ({
                        id: tag.id,
                        name: tag.name,
                    }))
                );
            } else {
                setError("Не удалось загрузить теги");
            }
        } catch (err) {
            setError("Ошибка при загрузке тегов");
        }
    };

    const loadMoreRecipes = () => {
        setVisibleCount((prevCount) => prevCount + 4);
    };

    const handleLike = async (recipeId: number) => {
        try {
            const currentStatus = recipeStatuses[recipeId]?.recipeLiked;
            await likeRecipe(recipeId);
            const updatedRecipeStatus = await checkStatusLikeStarRecipe(
                recipeId
            );
            if (updatedRecipeStatus) {
                setRecipes((prevRecipes) =>
                    prevRecipes.map((recipe) =>
                        recipe.id === recipeId
                            ? {
                                  ...recipe,
                                  usersLikesCount: currentStatus
                                      ? (recipe.usersLikesCount || 0) - 1
                                      : (recipe.usersLikesCount || 0) + 1,
                              }
                            : recipe
                    )
                );
                setRecipeStatuses((prevStatuses) => ({
                    ...prevStatuses,
                    [recipeId]: {
                        ...prevStatuses[recipeId],
                        recipeLiked: !currentStatus,
                    },
                }));
            }
        } catch {
            setError("Ошибка при установке лайка");
        }
    };

    const handleStar = async (recipeId: number) => {
        try {
            const currentStatus = recipeStatuses[recipeId]?.recipeStarred;
            await starRecipe(recipeId);
            const updatedRecipeStatus = await checkStatusLikeStarRecipe(
                recipeId
            );
            if (updatedRecipeStatus) {
                setRecipes((prevRecipes) =>
                    prevRecipes.map((recipe) =>
                        recipe.id === recipeId
                            ? {
                                  ...recipe,
                                  usersStarsCount: currentStatus
                                      ? (recipe.usersStarsCount || 0) - 1
                                      : (recipe.usersStarsCount || 0) + 1,
                              }
                            : recipe
                    )
                );
                setRecipeStatuses((prevStatuses) => ({
                    ...prevStatuses,
                    [recipeId]: {
                        ...prevStatuses[recipeId],
                        recipeStarred: updatedRecipeStatus.recipeStarred,
                    },
                }));
            }
        } catch {
            setError("Ошибка при установке звезды");
        }
    };

    useEffect(() => {
        fetchPopularTags();
        const queryParams = new URLSearchParams(location.search);
        const query = queryParams.get("query");

        if (query) {
            performSearch(query);
        } else {
            fetchMostLikedRecipes();
        }
    }, [location.search]);

    return (
        <div className="recipes-page">
            <div className="recipes-page-header">
                <h1 className="recipes-page-title">Рецепты</h1>
                {isAuthenticated && (
                    <button
                        className="button-add-recipe"
                        onClick={handleAddRecipe}
                    >
                        <img src={plus} alt="plus" />
                        Добавить рецепт
                    </button>
                )}
            </div>
            <div className="popularTags-block">
                <button
                    className="tag-block"
                    onClick={() => handleSearch("Простые блюда")}
                >
                    <div className="tag-icon">
                        <img src={tagIcon1} alt="" />
                    </div>
                    <p className="popular-tag-name">Простые блюда</p>
                </button>
                <button
                    className="tag-block"
                    onClick={() => handleSearch("Детское")}
                >
                    <div className="tag-icon">
                        <img src={tagIcon2} alt="" />
                    </div>
                    <p className="popular-tag-name">Детское</p>
                </button>
                <button
                    className="tag-block"
                    onClick={() => handleSearch("От шеф-поваров")}
                >
                    <div className="tag-icon">
                        <img src={tagIcon3} alt="" />
                    </div>
                    <p className="popular-tag-name">От шеф-поваров</p>
                </button>
                <button
                    className="tag-block"
                    onClick={() => handleSearch("На праздник")}
                >
                    <div className="tag-icon">
                        <img src={tagIcon4} alt="" />
                    </div>
                    <p className="popular-tag-name">На праздник</p>
                </button>
            </div>
            <div className="recipes-search-block">
                <p className="search-block-title">Поиск рецепта</p>
                <div>
                    <input
                        className="search-block-input"
                        type="text"
                        value={searchString}
                        onChange={(e) => setSearchString(e.target.value)}
                        placeholder="Название Блюда..."
                    />
                    <PopularTags
                        tags={popularTags}
                        onTagClick={handleTagClick}
                    />
                </div>
                <button
                    className="search-block-button"
                    onClick={() => handleSearch(searchString)}
                >
                    Поиск
                </button>
            </div>
            {loading && <p>Загрузка рецептов...</p>}
            {error && <p className="error">{error}</p>}
            {recipes.length > 0 ? (
                <div>
                    <ul className="recipe-list">
                        {recipes.slice(0, visibleCount).map((recipe) => (
                            <RecipeCard
                                key={recipe.id}
                                recipe={recipe}
                                recipeStatus={recipeStatuses[recipe.id]}
                                onLike={handleLike}
                                onStar={handleStar}
                            />
                        ))}
                    </ul>
                    {visibleCount < recipes.length && (
                        <button
                            className="button-more-loading"
                            onClick={loadMoreRecipes}
                        >
                            Загрузить еще
                        </button>
                    )}
                </div>
            ) : (
                <p className="recipe-list-clear">Нет доступных рецептов.</p>
            )}
        </div>
    );
};

export default Recipes;
