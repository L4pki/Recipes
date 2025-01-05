import React, { useEffect, useState } from 'react';
import { infoUser , recipesUser, UpdateUser } from '../api/userService';
import './styles/Profile.css';
import { UserInfo, User } from '../types/user';
import { RecipeCard } from '../components/RecipeCard/RecipeCard';
import { Recipe, RecipeStatus } from '../types/recipe';
import { likeRecipe, starRecipe, checkStatusLikeStarRecipe } from '../api/recipeService';
import UserEditForm from '../components/forms/UserEditForm';
import { useNavigate } from 'react-router-dom';
import backspace from "../assets/images/backspace.png";
import edit from "../assets/images/edit.png";
import icmenu from "../assets/images/ic-menu.png";

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
          const recipesArray = response.recipes;
      
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
        setUser (prevUser  => {
          if (prevUser ) {
            return {
              ...prevUser ,
              likeRecipesCount: currentStatus ? (prevUser.likeRecipesCount || 0) - 1 : (prevUser.likeRecipesCount || 0) + 1,
            };
          }
          return null;
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
        setUser (prevUser  => {
          if (prevUser ) {
            return {
              ...prevUser ,
              favoriteRecipesCount: currentStatus ? (prevUser.favoriteRecipesCount || 0) - 1 : (prevUser.favoriteRecipesCount || 0) + 1,
            };
          }
          return null;
        });
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
      <button className='backspace-button' onClick={()=>navigate("/main")}>
        <img src={backspace} alt="Назад" />
        <p className='backspace-text'>Назад</p> 
      </button>
      <p className="profile-title">Мой профиль</p>
      <div className="user-info">
        <button onClick={() => setIsEditing(true)} className='user-info-edit'><img src={edit} alt="Редактировать" /></button>
        <div className='user-info-first'>
          <div className='user-info-first-block'>
            <p className='user-info-first-title'>Имя</p>
            <p className='user-info-text'> {user.name}</p>
          </div>
          <div className='user-info-first-block'>
            <p className='user-info-first-title'>Логин</p>
            <p className='user-info-text'> {user.login}</p>
          </div>
          <div className='user-info-first-block'>
            <p className='user-info-first-title'>Пароль</p>
            <p className='user-info-text'> ********</p>
          </div>
        </div>
        <div className='user-info-second'>
          <p className='user-info-title'>Напиши немного о себе</p>
          <p className='user-info-text'> {user.about}</p>
        </div>
      </div>
      <div className='user-recipe-count-container'>
        <div className='user-recipe-count'>
          <div className='user-recipe-about'>
            <div className='img-rectangle'>
              <img src={icmenu} alt="icmenu" />
            </div>
            <p className='count-text'>Всего рецептов</p>
          </div>
          <p className='count'>{user.personalRecipesCount}</p>
        </div>
        <div className='user-recipe-count'>
        <div className='user-recipe-about'>
            <div className='img-rectangle'>
              <img src={icmenu} alt="icmenu" />
            </div>
            <p className='count-text'>Всего лайков</p>
          </div>
          <p className='count'>{user.likeRecipesCount}</p>
        </div>
        <div className='user-recipe-count'>
        <div className='user-recipe-about'>
            <div className='img-rectangle'>
              <img src={icmenu} alt="icmenu" />
            </div>
            <p className='count-text'>В избранных</p>
          </div>
          <p className='count'>{user.favoriteRecipesCount}</p>
        </div>
      </div>

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


{personalRecipes.length > 0 ? (
  <>
    <h2 className='personal-recipe-title'>Мои рецепты</h2>
    <ul className="personal-recipes-list">
      {personalRecipes.map(recipe => (
        <RecipeForm
          key={recipe.id}
          recipe={recipe}
          recipeStatus={recipeStatuses[recipe.id]}
          onLike={handleLike}
          onStar={handleStar}
        />
      ))}
    </ul>
  </>
) : (
  <>
    <p className='personal-recipe-title'>Рецептов пока нет</p>
  </>
)}
    </div>
  );
}

export default Profile;
