import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "./styles/Main.css";
import { Recipe, Tag } from "../types/recipe";
import { GetMostLikedRecipes, GetPopularTagList } from "../api/recipeService";
import { BestRecipeCard } from "../components/RecipeCard/RecipeCard";
import PopularTags from "../components/forms/TagForm";
import headerImg from "../assets/images/header-img.png";
import plus from "../assets/images/plus-white.png";
import tagIcon1 from "../assets/images/tag-icon1.png";
import tagIcon2 from "../assets/images/tag-icon2.png";
import tagIcon3 from "../assets/images/tag-icon3.png";
import tagIcon4 from "../assets/images/tag-icon4.png";
import Popup from "../components/AuthPopup/AuthPopup";

const Main: React.FC = () => {
    const navigate = useNavigate();
    const [recipes, setRecipes] = useState<Recipe[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [searchString, setSearchString] = useState<string>("");
    const [popularTags, setPopularTags] = useState<Tag[]>([]);
    const [isPopupOpen, setIsPopupOpen] = useState<boolean>(false);
    const [isAuthenticated, setIsAuthenticated] = useState(false);

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
            } else {
                setError("Не удалось получить избранные рецепты");
            }
        } catch (err) {
            setError("Ошибка при загрузке рецептов");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        setIsAuthenticated(!!localStorage.getItem("token"));
        fetchMostLikedRecipes();
        fetchPopularTags();
    }, []);

    const handleSearch = async (searchTerm: string) => {
        navigate(`/recipes?query=${encodeURIComponent(searchTerm)}`);
    };

    const handleTagClick = (tag: string) => {
        setSearchString(tag);
        handleSearch(tag);
    };

    const handleSuccessfulAuth = () => {
        setIsAuthenticated(true);
        setIsPopupOpen(false);
        window.location.reload();
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

    return (
        <div className="main-page">
            <div className="main-header-block">
                <div className="main-header-welcome">
                    <p className="main-header-title">
                        Готовь и делись рецептами
                    </p>
                    <p className="main-header-text">
                        Никаких кулинарных книг и блокнотов! Храни все любимые
                        рецепты в одном месте.
                    </p>
                    <div className="main-header-buttons">
                        <button
                            className="button-add-recipe"
                            onClick={handleAddRecipe}
                        >
                            <img src={plus} alt="plus" />
                            Добавить рецепт
                        </button>
                        {!isAuthenticated && (
                            <button
                                className="button-login"
                                onClick={() => setIsPopupOpen(true)}
                            >
                                Войти
                            </button>
                        )}
                        <Popup
                            isOpen={isPopupOpen}
                            isLogin={"login"}
                            onClose={() => setIsPopupOpen(false)}
                            onSuccessfulAuth={handleSuccessfulAuth}
                        />
                    </div>
                </div>

                <img
                    className="main-header-img"
                    src={headerImg}
                    alt="Изображение главной страницы"
                />
            </div>
            <p className="tag-title">Умная сортировка по тегам</p>
            <p className="tag-text">
                Добавляй рецепты и указывай наиболее популярные теги. Это
                позволит быстро находить любые категории.
            </p>
            <div className="popularTags-block">
                <button
                    className="tag-block"
                    onClick={() => handleSearch("Простые блюда")}
                >
                    <div className="tag-icon">
                        <img src={tagIcon1} alt="" />
                    </div>
                    <p className="popular-tag-name">Простые блюда</p>
                    <p className="popular-tag-text">
                        Время приготвления таких блюд не более 1 часа
                    </p>
                </button>
                <button
                    className="tag-block"
                    onClick={() => handleSearch("Детское")}
                >
                    <div className="tag-icon">
                        <img src={tagIcon2} alt="" />
                    </div>
                    <p className="popular-tag-name">Детское</p>
                    <p className="popular-tag-text">
                        Самые полезные блюда которые можно детям любого возраста
                    </p>
                </button>
                <button
                    className="tag-block"
                    onClick={() => handleSearch("От шеф-поваров")}
                >
                    <div className="tag-icon">
                        <img src={tagIcon3} alt="" />
                    </div>
                    <p className="popular-tag-name">От шеф-поваров</p>
                    <p className="popular-tag-text">
                        Требуют умения, времени и терпения, зато как в ресторане
                    </p>
                </button>
                <button
                    className="tag-block"
                    onClick={() => handleSearch("На праздник")}
                >
                    <div className="tag-icon">
                        <img src={tagIcon4} alt="" />
                    </div>
                    <p className="popular-tag-name">На праздник</p>
                    <p className="popular-tag-text">
                        Чем удивить гостей, чтобы все были сыты за праздничным
                        столом
                    </p>
                </button>
            </div>
            {loading && <p>Загрузка рецептов...</p>}
            {error && <p className="error">{error}</p>}
            {recipes.length > 0 ? (
                <div>
                    <ul className="bestRecipe-list">
                        {recipes.map((recipe) => (
                            <BestRecipeCard key={recipe.id} recipe={recipe} />
                        ))}
                    </ul>
                </div>
            ) : (
                <p>Нет доступных рецептов.</p>
            )}
            <h1 className="search-block-title-main">Поиск рецептов</h1>
            <p className="search-block-text">
                Введите примерное название блюда, а мы по тегам найдем его
            </p>
            <div className="search-block">
                <div>
                    <input
                        className="search-block-input-main"
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
        </div>
    );
};

export default Main;
