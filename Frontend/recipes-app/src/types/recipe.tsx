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
    timeCosts: number;
    numberOfPersons: number;
    tags: Tag[];
    usersLikesCount: number; 
    usersStarsCount: number;
}

export interface RecipeDetail {
    id: number;
    authorName: string;
    name: string;
    shortDescription: string;
    photoUrl: string;
    timeCosts: number;
    numberOfPersons: number;
    tags: Tag[];
    ingridientForCooking: Ingredient[];
    stepOfCooking: Step[];
    usersLikesCount: number; 
    usersStarsCount: number;
}

export interface UpdateRecipe {
    idRecipe: number; 
    name: string;
    shortDescription: string; 
    photoUrl: string; 
    timeCosts: number;
    numberOfPersons: number;
    ingridients: UpdateIngredient[];
    steps: UpdateStep[];
    tags: UpdateTag[];
}

export interface CreateRecipe {
    name: string; 
    shortDescription: string;
    photoUrl: string; 
    timeCosts: number;
    numberOfPersons: number; 
    ingridients: UpdateIngredient[];
    steps: UpdateStep[]; 
    tags: UpdateTag[];
}

export interface ApiResponseRecipe {
    $id: string;
    recipes: Recipe[];
    message: string;
}
export interface RecipeStatus {
    recipeLiked?: boolean;
    recipeStarred?: boolean;
}

export interface UpdateIngredient {
    title: string; 
    description: string;
}

export interface UpdateStep {
    numberOfStep: number; 
    description: string; 
}

export interface UpdateTag {
    name: string; 
}