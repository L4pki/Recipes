import { Recipe } from "./recipe";

export interface User {
    login: string;
    password: string;
    name: string;
    about: string;
}

export interface PopupStatus {
    status: "login" | "regist" | "choise" | "change";
}

export interface UserInfo {
    id: string;
    login: string;
    passwordHash: string | null;
    name: string;
    about: string;
    favoriteRecipesCount: number;
    likeRecipesCount: number;
    personalRecipes: {
        $id: string;
        $values: Recipe[];
    };
    personalRecipesCount: number; 
}