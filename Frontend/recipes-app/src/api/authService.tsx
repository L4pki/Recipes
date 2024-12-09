import { User } from "../types/user";
import axiosInstance from "./axiosInstance";
import handleError from "../utils/errorHandler";

export const registerUser  = async (userData: User) => {
    try {
        const response = await axiosInstance.post("/auth/register", userData);
        console.log("Успешно зарегистрирован:", response.data);
        localStorage.setItem("token", response.data.token);
        return response.data;
    } catch (error: any) {
        return handleError(error); 
    }
};

export const loginUser  = async (loginData: { login: string; password: string; }) => {
    try {
        const response = await axiosInstance.post("/auth/login", loginData);
        console.log("Успешный вход:", response.data);
        localStorage.setItem("token", response.data.token);
        return response.data;
    } catch (error: any) {
        return handleError(error); 
    }
};

export const refreshAccessToken = async () => {
    try {
        const response = await axiosInstance.get('/auth/token');
        localStorage.setItem('token', response.data.token);
        console.log("Успешный вход:", response.data);
    } catch (error) {
        localStorage.removeItem("token");
        console.error('Ошибка обновления токена:', error);
    }
}
