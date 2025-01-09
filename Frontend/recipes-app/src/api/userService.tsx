import axiosInstance from "./axiosInstance";
import handleError from "../utils/errorHandler";
import { User } from "../types/user";

export const infoUser = async () => {
    try {
        const response = await axiosInstance.get("/user/info");
        console.log("Данные пользователя:", response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
    }
};

export const UpdateUser = async (user: User) => {
    try {
        const response = await axiosInstance.post("/user/update", user);
        console.log("Данные пользователя обновлены:", response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
    }
};

export const recipesUser = async () => {
    try {
        const response = await axiosInstance.get("/user/recipes");
        console.log("Получены рецепты по автору:", response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
    }
};
