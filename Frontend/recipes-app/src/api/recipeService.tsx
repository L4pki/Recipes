import {
    ApiResponseRecipe,
    CreateRecipe,
    RecipeStatus,
    UpdateRecipe,
} from "../types/recipe";
import handleError from "../utils/errorHandler";
import axiosInstance from "./axiosInstance";

export const getFavoriteRecipes = async (): Promise<
    ApiResponseRecipe | undefined
> => {
    try {
        const response = await axiosInstance.get<ApiResponseRecipe>(
            "/user/recipe/star"
        );
        console.log("Рецепты получены:", response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
        console.error(
            "Ошибка при получении избранных рецептов:",
            error.response ? error.response.data : error.message
        );
        return undefined;
    }
};

export const checkStatusLikeStarRecipe = async (
    id: number
): Promise<RecipeStatus | undefined> => {
    try {
        const response = await axiosInstance.get<RecipeStatus>(
            `/recipe/status/${id}`
        );
        console.log("Рецепт:", "id", id, response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
        console.error(
            "Ошибка при получении Статуса:",
            error.response ? error.response.data : error.message
        );
        return undefined;
    }
};

export const likeRecipe = async (id: number) => {
    try {
        const response = await axiosInstance.post(`/recipe/like/${id}`);
        console.log("Лайк:", response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
        console.error(
            "Ошибка лайка:",
            error.response ? error.response.data : error.message
        );
        return undefined;
    }
};

export const starRecipe = async (id: number) => {
    try {
        const response = await axiosInstance.post(`/user/recipe/star/${id}`);
        console.log("Звезда:", response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
        console.error(
            "Ошибка лайка:",
            error.response ? error.response.data : error.message
        );
        return undefined;
    }
};

export const getRecipeDetail = async (id: number) => {
    try {
        const response = await axiosInstance.get(`/recipe/detail/${id}`);
        console.log("Рецепт детальная:", response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
        console.error(
            "Ошибка получения деталей:",
            error.response ? error.response.data : error.message
        );
        return undefined;
    }
};

export const UpdateRecipeApi = async (
    id: number,
    updatedRecipe: UpdateRecipe,
    newImageFile?: File
) => {
    try {
        const formData = new FormData();
        
        formData.append("Recipe.Name", updatedRecipe.name);
    formData.append("Recipe.ShortDescription", updatedRecipe.shortDescription);
    formData.append("Recipe.TimeCosts", updatedRecipe.timeCosts.toString());
    formData.append("Recipe.NumberOfPersons", updatedRecipe.numberOfPersons.toString());
    formData.append("Recipe.Ingridients", JSON.stringify(updatedRecipe.ingridients));
    formData.append("Recipe.Steps", JSON.stringify(updatedRecipe.steps));
    formData.append("Recipe.Tags", JSON.stringify(updatedRecipe.tags));

        if (newImageFile) {
            formData.append("photo", newImageFile);
        } else {
            formData.append("photoUrl", updatedRecipe.photoUrl || '');
        }

        const response = await axiosInstance.post(
            `/recipe/update/${id}`,
            formData,
            {
                headers: {
                    'Content-Type': 'multipart/form-data',
                },
            }
        );
        console.log("Рецепт обновлен:", response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
        console.error(
            "Ошибка обновления рецепта:",
            error.response ? error.response.data : error.message
        );
        return undefined;
    }
};

export const CreateRecipeApi = async (recipe: CreateRecipe, imageFile: File): Promise<ApiResponseRecipe | undefined> => {
    const formData = new FormData();
    
    formData.append("Recipe.Name", recipe.name);
    formData.append("Recipe.ShortDescription", recipe.shortDescription);
    formData.append("Recipe.TimeCosts", recipe.timeCosts.toString());
    formData.append("Recipe.NumberOfPersons", recipe.numberOfPersons.toString());
    formData.append("Recipe.Ingridients", JSON.stringify(recipe.ingridients));
    formData.append("Recipe.Steps", JSON.stringify(recipe.steps));
    formData.append("Recipe.Tags", JSON.stringify(recipe.tags));
    formData.append("Image", imageFile);
    try {
        const response = await axiosInstance.post<ApiResponseRecipe>(`/recipe/create`, formData, {
            headers: {
                "Content-Type": "multipart/form-data",
            },
        });
        console.log("Рецепт обновлен:", response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
        console.error(
            "Ошибка обновления рецепта:",
            error.response ? error.response.data : error.message
        );
        return undefined;
    }
};

export const GetTagAllList = async () => {
    try {
        const response = await axiosInstance.get(`/recipe/tag/getall`);
        console.log("Теги:", response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
        console.error(
            "Ошибка получения тегов:",
            error.response ? error.response.data : error.message
        );
        return undefined;
    }
};

export const GetPopularTagList = async () => {
    try {
        const response = await axiosInstance.get(`/recipe/tag/getpopular`);
        console.log("Теги:", response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
        console.error(
            "Ошибка получения тегов:",
            error.response ? error.response.data : error.message
        );
        return undefined;
    }
};

export const GetMostLikedRecipes = async (): Promise<
    ApiResponseRecipe | undefined
> => {
    try {
        const response = await axiosInstance.get<ApiResponseRecipe>(
            `/recipe/mostliked`
        );
        console.log("Залайканные рецепты:", response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
        console.error(
            "Ошибка получения рецептов:",
            error.response ? error.response.data : error.message
        );
        return undefined;
    }
};

export const SearchRecipes = async (searchString: string): Promise<
    ApiResponseRecipe | undefined
> => {
    try {
        const response = await axiosInstance.get<ApiResponseRecipe>(
            `/recipe/search/${searchString}`
        );
        console.log("Найденные рецепты:", response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
        console.error(
            "Ошибка получения рецептов:",
            error.response ? error.response.data : error.message
        );
        return undefined;
    }
};