export interface Tag {
    id: string; 
    name: string; 
}

export interface Ingredient {
    id: string;
    title: string;
    description: string;
}

export interface Step {
    id: string;
    description: string;
    numberOfStep: string;
}

export interface Recipe {
    id: number;
    authorName: string;
    name: string;
    shortDescription: string;
    photoUrl: string;
    timeCosts: string;
    numberOfPersons: number;
    tags: { $id: string; $values: Tag[] };
    usersLikesCount: number; 
    usersStarsCount: number;
}

export interface RecipeDetail {
    id: number;
    authorName: string;
    name: string;
    shortDescription: string;
    photoUrl: string;
    timeCosts: string;
    numberOfPersons: number;
    tags: { $id: string; $values: Tag[] };
    ingridientForCooking: { $id: string; $values: Ingredient[] };
    stepOfCooking: { $id: string; $values: Step[] };
    usersLikesCount: number; 
    usersStarsCount: number;
}

export interface UpdateRecipe {
    idRecipe: number; // ID рецепта
    name: string; // Название рецепта
    shortDescription: string; // Краткое описание рецепта
    photoUrl: string; // URL фотографии рецепта
    timeCosts: string; // Время приготовления в формате "HH:MM:SS"
    numberOfPersons: number; // Количество порций
    ingridients: UpdateIngredient[]; // Массив ингредиентов
    steps: UpdateStep[]; // Массив шагов приготовления
    tags: UpdateTag[]; // Массив тегов
}

export interface CreateRecipe {
    name: string; // Название рецепта
    shortDescription: string; // Краткое описание рецепта
    photoUrl: string; // URL фотографии рецепта
    timeCosts: string; // Время приготовления в формате "HH:MM:SS"
    numberOfPersons: number; // Количество порций
    ingridients: UpdateIngredient[]; // Массив ингредиентов
    steps: UpdateStep[]; // Массив шагов приготовления
    tags: UpdateTag[]; // Массив тегов
}

export interface ApiResponseRecipe {
    $id: string;
    recipes: {
        $id: string;
        $values: Recipe[];
    };
    message: string;
}
export interface RecipeStatus {
    recipeLiked?: boolean;
    recipeStarred?: boolean;
}

export interface UpdateIngredient {
    title: string; // Название ингредиента
    description: string; // Описание ингредиента (если нужно)
}

export interface UpdateStep {
    numberOfStep: number; // Номер шага
    description: string; // Описание шага
}

export interface UpdateTag {
    name: string; // Название тега
}