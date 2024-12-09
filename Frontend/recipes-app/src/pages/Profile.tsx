import React, { useEffect, useState } from 'react';
import { infoUser , recipesUser, UpdateUser } from '../api/userService';
import './styles/Profile.css';
import { UserInfo, User } from '../types/user';
import RecipeForm from '../components/RecipeCard/RecipeCard';
import { Recipe, RecipeStatus } from '../types/recipe';
import { likeRecipe, starRecipe, checkStatusLikeStarRecipe } from '../api/recipeService';
import UserEditForm from '../components/forms/UserEditForm';
import { useNavigate } from 'react-router-dom';

const Profile: React.FC = () => {
  const [user, setUser ] = useState<UserInfo | null>(null);
  const navigate = useNavigate();
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [isEditing, setIsEditing] = useState<boolean>(false);
  const [editUser , setEditUser ] = useState<User | null>(null);
  const [validationErrors, setValidationErrors] = useState({
    login: '',
    password: '',
  });
  const [recipeStatuses, setRecipeStatuses] = useState<{ [key: number]: RecipeStatus | undefined }>({});
  const [personalRecipes, setPersonalRecipes] = useState<Recipe[]>([]);

  useEffect(() => {
    const fetchUserInfoAndRecipes = async () => {
        try {
          const userInfo = await infoUser ();
          setUser (userInfo);
          setEditUser (userInfo);
      
          const response = await recipesUser (); // Получаем ответ от сервера
          console.log('Полученные рецепты:', response); // Логируем полученные рецепты
      
          // Извлекаем массив рецептов из ответа
          const recipesArray = response.recipes.$values;
      
          // Проверяем, является ли recipesArray массивом
          if (!Array.isArray(recipesArray)) {
            throw new TypeError('recipesArray не является массивом');
          }
      
          setPersonalRecipes(recipesArray);
      
          const statuses = await Promise.all(recipesArray.map(recipe => checkStatusLikeStarRecipe(recipe.id)));
      
          const statusesMap = recipesArray.reduce((acc, recipe, index) => {
            acc[recipe.id] = statuses[index];
            return acc;
          }, {});
      
          setRecipeStatuses(statusesMap);
        } catch (err) {
          console.error('Ошибка при загрузке данных:', err); // Логируем ошибку
          setError('Ошибка при загрузке данных о пользователе или рецептах');
        } finally {
          setLoading(false);
        }
    };
    fetchUserInfoAndRecipes();
    }, []);
      

  const handleUpdateUser  = async () => {
    if (editUser ) {
      try {
        await UpdateUser (editUser );
        const updatedUser  = await infoUser ();
        setUser (updatedUser );
        setIsEditing(false);
        setError(null);
      } catch (err) {
        setError('Ошибка при обновлении данных о пользователе');
      }
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;

    if (editUser ) {
      setEditUser ({ ...editUser , [name]: value });
    }

    // Валидация полей
    if (name === 'login' && !value) {
      setValidationErrors({ ...validationErrors, login: 'Логин обязателен для заполнения' });
    } else if (name === 'password') {
      if (value.length < 6) {
        setValidationErrors({ ...validationErrors, password: 'Пароль должен содержать не менее 6 символов' });
      } else {
        setValidationErrors({ ...validationErrors, password: '' });
      }
    } else {
      setValidationErrors({ ...validationErrors, [name]: '' });
    }
  };

  const isValid = () => {
    return !validationErrors.login && !validationErrors.password;
  };

  const handleLike = async (recipeId: number) => {
    try {
      const currentStatus = recipeStatuses[recipeId]?.recipeLiked;
      await likeRecipe(recipeId);
      const updatedRecipeStatus = await checkStatusLikeStarRecipe(recipeId);

      if (updatedRecipeStatus) {
        setPersonalRecipes(prevRecipes =>
          prevRecipes.map(recipe =>
            recipe.id === recipeId ? {
              ...recipe,
              usersLikesCount: currentStatus ? (recipe.usersLikesCount || 0) - 1 : (recipe.usersLikesCount || 0) + 1
            } : recipe
          )
        );
      setRecipeStatuses(prevStatuses => {
        const previousStatus = prevStatuses[recipeId] || {
          recipeLiked: false,
          likesCount: 0,
          starsCount: 0,
          recipeStarred: false,
        };

        return {
          ...prevStatuses,
          [recipeId]: {
            ...previousStatus,
            recipeLiked: !currentStatus,
            likesCount: updatedRecipeStatus?.recipeLiked || 0,
          }
        };
      });
    }
    } catch (err) {
      console.error(err);
      setError('Ошибка при установке лайка');
    }
  };

  const handleStar = async (recipeId: number) => {
    try {
      const currentStatus = recipeStatuses[recipeId]?.recipeStarred;
      await starRecipe(recipeId);
      const updatedRecipeStatus = await checkStatusLikeStarRecipe(recipeId);

      if (updatedRecipeStatus) {
        setPersonalRecipes(prevRecipes =>
          prevRecipes.map(recipe =>
            recipe.id === recipeId ? {
              ...recipe,
              usersStarsCount: currentStatus ? (recipe.usersStarsCount || 0) - 1 : (recipe.usersStarsCount || 0) + 1
            } : recipe
          )
        );
        setRecipeStatuses(prevStatuses => ({
          ...prevStatuses,
          [recipeId]: {
            ...prevStatuses[recipeId],
            recipeStarred: updatedRecipeStatus?.recipeStarred,
            starsCount: updatedRecipeStatus?.recipeStarred || 0,
          }
        }));
      }
    } catch (err) {
      console.error('Error in handleStar:', err);
      setError('Ошибка при установке звезды');
    }
  };

  if (loading) {
    return <div className="loading">Загрузка...</div>;
  }

  if (error) {
    return <div className="error">{error}</div>;
  }

  if (!user) {
    return <div className="error">Пользователь не найден.</div>;
  }

  return (
    <div className="profile-container">
      <h1>Профиль пользователя</h1>
      <div className="user-info">
        <p><strong>Имя:</strong> {user.name}</p>
        <p><strong>Логин:</strong> {user.login}</p>
        <p><strong>О себе:</strong> {user.about}</p>
        <p><strong>Количество любимых рецептов:</strong> {user.favoriteRecipesCount}</p>
        <p><strong>Количество лайкнутых рецептов:</strong> {user.likeRecipesCount}</p>
        <p><strong>Количество собственных рецептов:</strong> {user.personalRecipesCount}</p>
      </div>
      <button onClick={() => setIsEditing(true)} className="update-button">Редактировать данные</button>
      <button onClick={() => navigate(`/RecipeCreate`)}>Добавить рецепт</button>

      {isEditing && editUser  && (
        <UserEditForm
          editUser ={editUser }
          validationErrors={validationErrors}
          handleChange={handleChange}
          handleUpdateUser ={handleUpdateUser }
          setIsEditing={setIsEditing}
          isValid={isValid}
        />
      )}

      <h2>Мои персональные рецепты</h2>
      <ul className="personal-recipes-list">
        {personalRecipes.length > 0 ? (
          personalRecipes.map(recipe => (
            <RecipeForm
              key={recipe.id}
              recipe={recipe}
              recipeStatus={recipeStatuses[recipe.id]}
              onLike={handleLike}
              onStar={handleStar}
            />
          ))
        ) : (
          <li>Нет персональных рецептов.</li>
        )}
      </ul>
    </div>
  );
}

export default Profile;
