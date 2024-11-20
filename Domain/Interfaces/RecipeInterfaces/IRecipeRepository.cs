using Domain.Entities.RecipeEntities;

namespace Domain.Interfaces.RecipeInterfaces;

public interface IRecipeRepository
{
    Task<Recipe> CreateAsync( Recipe recipe, CancellationToken cancellationToken );

    Task<string> DeleteAsync( int id, CancellationToken cancellationToken );

    Task<Recipe> UpdateAsync( int id, Recipe recipe, CancellationToken cancellationToken );

    Task<Recipe> GetByIdAsync( int id, CancellationToken cancellationToken );

    Task<string> LikeAsync( int idUser, int idLikedRecipe, CancellationToken cancellationToken );

    Task<IReadOnlyList<Recipe>> GetFavoriteRecipesAsync( int idUser, CancellationToken cancellationToken );

    Task<IReadOnlyList<Recipe>> GetLikedRecipesAsync( int userId, CancellationToken cancellationToken );

    Task<IReadOnlyList<Recipe>> GetByTagsOrNameAsync( string searchQuery, CancellationToken cancellationToken );

    Task<IReadOnlyList<Recipe>> GetAllAsync( CancellationToken cancellationToken );

    Task<IReadOnlyList<Recipe>> GetByAuthorAsync( int idAuthor, CancellationToken cancellationToken );

    Task<IReadOnlyList<Recipe>> GetMostLikedRecipesAsync( CancellationToken cancellationToken );
}
