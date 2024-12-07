import React from "react";
import { Link } from "react-router-dom";
import { Recipe, RecipeStatus } from "../../types/recipe";
import StarOn from "../../assets/images/StarOn.png";
import StarOff from "../../assets/images/StarOff.png";
import LikeOff from "../../assets/images/LikeOff.png";
import LikeOn from "../../assets/images/LikeOn.png";
import StopWatch from "../../assets/images/Stopwatch.png";
import Person from "../../assets/images/Person.png";
import "../RecipeCard/RecipeCard.css";

interface RecipeFormProps {
    recipe: Recipe;
    recipeStatus: RecipeStatus | undefined;
    onLike: (recipeId: number) => void;
    onStar: (recipeId: number) => void;
}

const minutesFromTimeString = (timeString: string) => {
    const [hours, minutes, seconds] = timeString.split(":").map(Number);
    const totalMinutes = hours * 60 + minutes + Math.floor(seconds / 60);
    return totalMinutes.toString();
};

const RecipeCard: React.FC<RecipeFormProps> = ({
    recipe,
    recipeStatus,
    onLike,
    onStar,
}) => {
    const starImage = recipeStatus?.recipeStarred ? StarOn : StarOff;
    const likeImage = recipeStatus?.recipeLiked ? LikeOn : LikeOff;
    return (
        <li key={recipe.id} className="recipe-content">
            <div className="recipe-item">
                <Link to={`/detail/${recipe.id}`} className="recipe-link">
                    <div className="recipe-container">
                        {recipe.photoUrl && (
                            <img
                                src={recipe.photoUrl}
                                alt={recipe.name}
                                className="recipe-image"
                            />
                        )}
                        <p className="recipe-author">@{recipe.authorName}</p>
                    </div>
                </Link>
                <div className="recipe-about">
                    <div className="recipe-info-first">
                        <div className="recipe-tags">
                            {recipe.tags?.$values?.length > 0 ? (
                                recipe.tags.$values.map((tag) => (
                                    <div className="recipe-tag" key={tag.id}>
                                        {" "}
                                        <span className="recipe-tag-text">
                                            {tag.name}
                                        </span>
                                    </div>
                                ))
                            ) : (
                                <span></span>
                            )}
                        </div>
                        <div className="recipe-button">
                            <p>
                                <button
                                    onClick={(e) => {
                                        e.stopPropagation();
                                        onLike(recipe.id);
                                    }}
                                    className="starLike-button"
                                >
                                    <img
                                        className="starLike-button-image"
                                        src={likeImage}
                                        alt={
                                            recipeStatus?.recipeLiked
                                                ? "Liked"
                                                : "Not Liked"
                                        }
                                    />
                                    <h3 className="starLike-button-text">
                                        {recipe.usersLikesCount}
                                    </h3>
                                </button>
                            </p>
                            <p>
                                <button
                                    onClick={(e) => {
                                        e.stopPropagation();
                                        onStar(recipe.id);
                                    }}
                                    className="starLike-button"
                                >
                                    <img
                                        className="starLike-button-image"
                                        src={starImage}
                                        alt={
                                            recipeStatus?.recipeStarred
                                                ? "Starred"
                                                : "Not Starred"
                                        }
                                    />
                                    <h3 className="starLike-button-text">
                                        {recipe.usersStarsCount}
                                    </h3>
                                </button>
                            </p>
                        </div>
                    </div>
                    <div className="recipe-info-second">
                        <h3 className="recipe-name">{recipe.name}</h3>
                        <h3 className="recipe-description">
                            {recipe.shortDescription}
                        </h3>
                    </div>
                    <div className="recipe-info-third">
                        <div className="recipe-time">
                            <img src={StopWatch} alt="Часы" />
                            <div className="recipe-time-textbox">
                                <p className="recipe-time-text">
                                    Время приготовления:
                                </p>
                                <p className="recipe-time-value">
                                    {minutesFromTimeString(recipe.timeCosts)}{" "}
                                    минут
                                </p>
                            </div>
                        </div>
                        <div className="recipe-person">
                            <img src={Person} alt="Количество персон" />
                            <div className="recipe-person-textbox">
                                <p className="recipe-person-text">Рецепт на:</p>
                                <p className="recipe-person-value">
                                    {recipe.numberOfPersons} персон{" "}
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </li>
    );
};

export default RecipeCard;
