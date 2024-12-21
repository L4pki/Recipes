import { Recipe } from "./recipe";

export interface User {
    login: string;
    password: string;
    name: string;
    about: string;
}

export interface PopupStatus {
    status: "login" | "regist" | "choise";
}

export interface UserInfo {
    id: string; // Добавляем идентификатор пользователя
    login: string;
    passwordHash: string | null; // Используем passwordHash вместо password
    name: string;
    about: string;
    favoriteRecipesCount: number;
    likeRecipesCount: number;
    personalRecipes: {
        $id: string; // Или другой тип, в зависимости от вашего API
        $values: Recipe[];
    };
    personalRecipesCount: number; // Количество личных рецептов
}