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
        //console.log("Рецепты получены:", response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
        /*console.error(
            "Ошибка при получении избранных рецептов:",
            error.response ? error.response.data : error.message
        );*/
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
        return response.data;
    } catch (error: any) {
        handleError(error);
        /*console.error(
            "Ошибка при получении Статуса:",
            error.response ? error.response.data : error.message
        );*/
        return undefined;
    }
};

export const likeRecipe = async (id: number) => {
    try {
        const response = await axiosInstance.post(`/recipe/like/${id}`);
        //console.log("Лайк:", response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
        /*console.error(
            "Ошибка лайка:",
            error.response ? error.response.data : error.message
        );*/
        return undefined;
    }
};

export const starRecipe = async (id: number) => {
    try {
        const response = await axiosInstance.post(`/user/recipe/star/${id}`);
        //console.log("Звезда:", response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
        /*console.error(
            "Ошибка лайка:",
            error.response ? error.response.data : error.message
        );*/
        return undefined;
    }
};

export const getRecipeDetail = async (id: number) => {
    try {
        const response = await axiosInstance.get(`/recipe/detail/${id}`);
        //console.log("Рецепт детальная:", response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
        /*console.error(
            "Ошибка получения деталей:",
            error.response ? error.response.data : error.message
        );*/
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
        formData.append(
            "Recipe.ShortDescription",
            updatedRecipe.shortDescription
        );
        formData.append("Recipe.TimeCosts", "00:30:00");
        formData.append(
            "Recipe.NumberOfPersons",
            updatedRecipe.numberOfPersons.toString()
        );
        updatedRecipe.ingridients.forEach((ingredient, index) => {
            formData.append(
                `Recipe.Ingridients[${index}].Title`,
                ingredient.title
            );
            formData.append(
                `Recipe.Ingridients[${index}].Description`,
                ingredient.description
            );
        });

        updatedRecipe.steps.forEach((step, index) => {
            formData.append(
                `Recipe.Steps[${index}].NumberOfStep`,
                step.numberOfStep.toString()
            );
            formData.append(
                `Recipe.Steps[${index}].Description`,
                step.description
            );
        });

        updatedRecipe.tags.forEach((tag, index) => {
            formData.append(`Recipe.Tags[${index}].Name`, tag.name);
        });

        if (newImageFile) {
            formData.append("Image", newImageFile);
        } else {
            formData.append("photoUrl", updatedRecipe.photoUrl || "");
        }
        //console.log("Рецепт =======:", formData);
        /*Array.from(formData.entries()).forEach(([key, value]) => {
            console.log(`${key}: ${value}`);
        });*/
        const response = await axiosInstance.post(
            `/recipe/update/${id}`,
            formData,
            {
                headers: {
                    "Content-Type": "multipart/form-data",
                },
            }
        );
        //console.log("Рецепт обновлен:", response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
        /*console.error(
            "Ошибка обновления рецепта:",
            error.response ? error.response.data : error.message
        );*/
        return undefined;
    }
};

export const DeleteRecipe = async (
    idRecipe: number
): Promise<string | undefined> => {
    try {
        const response = await axiosInstance.delete<string>(
            `/recipe/delete/${idRecipe}`
        );
        //console.log("Рецепт удален:", response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
        /*console.error(
            "Ошибка удаления рецепта:",
            error.response ? error.response.data : error.message
        );*/
        return undefined;
    }
};

export const CreateRecipeApi = async (
    recipe: CreateRecipe,
    imageFile: File
): Promise<ApiResponseRecipe | undefined> => {
    const formData = new FormData();

    formData.append("Recipe.Name", recipe.name);
    formData.append("Recipe.ShortDescription", recipe.shortDescription);
    formData.append("Recipe.TimeCosts", recipe.timeCosts.toString());
    formData.append(
        "Recipe.NumberOfPersons",
        recipe.numberOfPersons.toString()
    );
    recipe.ingridients.forEach((ingredient, index) => {
        formData.append(`Recipe.Ingridients[${index}].Title`, ingredient.title);
        formData.append(
            `Recipe.Ingridients[${index}].Description`,
            ingredient.description
        );
    });

    recipe.steps.forEach((step, index) => {
        formData.append(
            `Recipe.Steps[${index}].NumberOfStep`,
            step.numberOfStep.toString()
        );
        formData.append(`Recipe.Steps[${index}].Description`, step.description);
    });

    recipe.tags.forEach((tag, index) => {
        formData.append(`Recipe.Tags[${index}].Name`, tag.name);
    });
    formData.append("Image", imageFile);
    try {
        const response = await axiosInstance.post<ApiResponseRecipe>(
            `/recipe/create`,
            formData,
            {
                headers: {
                    "Content-Type": "multipart/form-data",
                },
            }
        );
        //console.log("Рецепт обновлен:", response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
        /*console.error(
            "Ошибка обновления рецепта:",
            error.response ? error.response.data : error.message
        );*/
        return undefined;
    }
};

export const GetTagAllList = async () => {
    try {
        const response = await axiosInstance.get(`/recipe/tag/getall`);
        //console.log("Теги:", response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
        /*console.error(
            "Ошибка получения тегов:",
            error.response ? error.response.data : error.message
        );*/
        return undefined;
    }
};

export const GetPopularTagList = async () => {
    try {
        const response = await axiosInstance.get(`/recipe/tag/getpopular`);
        //console.log("Теги:", response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
        /*console.error(
            "Ошибка получения тегов:",
            error.response ? error.response.data : error.message
        );*/
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
        //console.log("Залайканные рецепты:", response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
        /*console.error(
            "Ошибка получения рецептов:",
            error.response ? error.response.data : error.message
        );*/
        return undefined;
    }
};

export const SearchRecipes = async (
    searchString: string
): Promise<ApiResponseRecipe | undefined> => {
    try {
        const response = await axiosInstance.get<ApiResponseRecipe>(
            `/recipe/search/${searchString}`
        );
        //console.log("Найденные рецепты:", response.data);
        return response.data;
    } catch (error: any) {
        handleError(error);
        /*console.error(
            "Ошибка получения рецептов:",
            error.response ? error.response.data : error.message
        );*/
        return undefined;
    }
};
