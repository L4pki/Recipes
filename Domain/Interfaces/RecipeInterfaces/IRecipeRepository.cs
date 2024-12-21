using Domain.Entities.RecipeEntities;
using Domain.Models;

namespace Domain.Interfaces.RecipeInterfaces;

public interface IRecipeRepository
{
    Task<Recipe> CreateAsync( Recipe recipe, CancellationToken cancellationToken );

    Task<string> DeleteAsync( int id, CancellationToken cancellationToken );

    Task<Recipe> UpdateAsync( int id, Recipe recipe, CancellationToken cancellationToken );

    Task<Recipe> GetByIdAsync( int id, CancellationToken cancellationToken );

    Task<RecipeModel> GetDetailByIdAsync ( int id, CancellationToken cancellationToken );

    Task<string> LikeAsync( int idUser, int idLikedRecipe, CancellationToken cancellationToken );

    Task<IReadOnlyList<bool>> RecipeLikeStarStatus(int idUser, int recipeId, CancellationToken cancellationToken );

    Task<IReadOnlyList<RecipeModel>> GetFavoriteRecipesAsync( int idUser, CancellationToken cancellationToken );

    Task<IReadOnlyList<RecipeModel>> GetLikedRecipesAsync( int userId, CancellationToken cancellationToken );

    Task<IReadOnlyList<RecipeModel>> GetByTagsOrNameAsync( string searchQuery, CancellationToken cancellationToken );

    Task<IReadOnlyList<RecipeModel>> GetAllAsync( CancellationToken cancellationToken );

    Task<IReadOnlyList<RecipeModel>> GetByAuthorAsync( int idAuthor, CancellationToken cancellationToken );

    Task<IReadOnlyList<RecipeModel>> GetMostLikedRecipesAsync( CancellationToken cancellationToken );
}
