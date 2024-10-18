using Domain.Entities;
using Domain.Entities.RecipeEntities;
using Domain.Interfaces.RecipeInterfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.RecipeRepositories;

public class RecipeRepository : IRecipeRepository
{
    private readonly RecipeDbContext _recipeDbContext;

    public RecipeRepository( RecipeDbContext recipeDbContext )
    {
        _recipeDbContext = recipeDbContext;
    }

    public async Task<Recipe> CreateAsync( Recipe recipe, CancellationToken cancellationToken )
    {
        await _recipeDbContext.Set<Recipe>().AddAsync( recipe, cancellationToken );
        await _recipeDbContext.SaveChangesAsync( cancellationToken );
        return recipe;
    }

    public async Task<string> DeleteAsync( int id, CancellationToken cancellationToken )
    {
        var recipe = await _recipeDbContext.Set<Recipe>()
        .Include( r => r.IngridientForCooking )
        .Include( r => r.StepOfCooking )
        .FirstOrDefaultAsync( r => r.Id == id, cancellationToken );
        if ( recipe != null )
        {
            _recipeDbContext.Set<Ingridient>().RemoveRange( recipe.IngridientForCooking );
            _recipeDbContext.Set<Step>().RemoveRange( recipe.StepOfCooking );
            _recipeDbContext.Remove( recipe );
            await _recipeDbContext.SaveChangesAsync( cancellationToken );
            return ( "Успешно удален" );
        }
        return ( "Рецепт не найден" );
    }

    public async Task<Recipe> UpdateAsync( int idRecipe, Recipe modifiedRecipe, CancellationToken cancellationToken )
    {
        var recipe = await _recipeDbContext.Set<Recipe>()
        .Include( r => r.Tags )
        .Include( r => r.IngridientForCooking )
        .Include( r => r.StepOfCooking )
        .FirstOrDefaultAsync( r => r.Id == idRecipe, cancellationToken );
        if ( recipe == null )
        {
            return await CreateAsync( modifiedRecipe, cancellationToken );
        }

        recipe.Name = modifiedRecipe.Name;
        recipe.ShortDescription = modifiedRecipe.ShortDescription;
        recipe.PhotoUrl = modifiedRecipe.PhotoUrl;
        recipe.TimeCosts = modifiedRecipe.TimeCosts;
        recipe.NumberOfPersons = modifiedRecipe.NumberOfPersons;

        recipe.Tags.Clear();
        recipe.IngridientForCooking.Clear();
        recipe.StepOfCooking.Clear();

        _recipeDbContext.Set<Ingridient>().RemoveRange( recipe.IngridientForCooking );
        _recipeDbContext.Set<Step>().RemoveRange( recipe.StepOfCooking );

        await _recipeDbContext.SaveChangesAsync( cancellationToken );
        return recipe;
    }

    public async Task<string> LikeAsync( int idUser, int idLikedRecipe, CancellationToken cancellationToken )
    {
        var recipe = await _recipeDbContext.Set<Recipe>()
            .Include( r => r.UsersLikes )
            .FirstOrDefaultAsync( r => r.Id == idLikedRecipe, cancellationToken );

        if ( recipe != null )
        {
            var user = await _recipeDbContext.Set<User>().FindAsync( idUser );
            if ( user != null )
            {
                if ( recipe.UsersLikes.Any( u => u.Id == idUser ) )
                {
                    recipe.UsersLikes.Remove( user );
                    await _recipeDbContext.SaveChangesAsync( cancellationToken );
                    return "Лайк удален";
                }
                else
                {
                    recipe.UsersLikes.Add( user );
                    await _recipeDbContext.SaveChangesAsync( cancellationToken );
                    return "Рецепт успешно лайкнут";
                }
            }
            return "Пользователь не найден";
        }
        return "Рецепт не найден";
    }

    public async Task<Recipe> GetByIdAsync( int id, CancellationToken cancellationToken )
    {
        var recipe = await _recipeDbContext.Set<Recipe>()
        .FirstOrDefaultAsync( r => r.Id == id, cancellationToken );
        if ( recipe != null )
        {
            return recipe;
        }
        return null;
    }

    public async Task<IReadOnlyList<Recipe>> GetByTagsOrNameAsync( string searchQuery, CancellationToken cancellationToken )
    {
        if ( string.IsNullOrWhiteSpace( searchQuery ) )
        {
            return new List<Recipe>().AsReadOnly();
        }

        var recipes = await _recipeDbContext.Set<Recipe>()
            .Include( r => r.Tags )
            .Where( r => r.Name.ToLower().Contains( searchQuery.ToLower() ) ||
                r.Tags.Any( t => t.Name.ToLower().Contains( searchQuery.ToLower() ) ) )
            .ToListAsync( cancellationToken );

        return recipes.AsReadOnly();
    }

    public async Task<IReadOnlyList<Recipe>> GetFavoriteRecipesAsync( int userId, CancellationToken cancellationToken )
    {
        var recipes = await _recipeDbContext.Users
            .Where( u => u.Id == userId )
            .SelectMany( u => u.FavoriteRecipes )
            .ToListAsync( cancellationToken );
        return recipes.AsReadOnly();
    }

    public async Task<IReadOnlyList<Recipe>> GetLikedRecipesAsync( int userId, CancellationToken cancellationToken )
    {
        var recipes = await _recipeDbContext.Users
            .Where( u => u.Id == userId )
            .SelectMany( u => u.LikeRecipes )
            .ToListAsync( cancellationToken );
        return recipes.AsReadOnly();
    }

    public async Task<IReadOnlyList<Recipe>> GetByAuthorAsync( int idAuthor, CancellationToken cancellationToken )
    {
        var recipes = await _recipeDbContext.Set<Recipe>()
                                    .Where( r => r.IdAuthor == idAuthor )
                                    .ToListAsync( cancellationToken );
        return recipes.AsReadOnly();
    }

    public async Task<IReadOnlyList<Recipe>> GetAllAsync( CancellationToken cancellationToken )
    {
        var recipes = await _recipeDbContext.Set<Recipe>().ToListAsync( cancellationToken );
        return recipes.AsReadOnly();
    }

    public async Task<IReadOnlyList<Recipe>> GetMostLikedRecipesAsync( CancellationToken cancellationToken )
    {
        var maxLikes = await _recipeDbContext.Set<Recipe>()
            .Select( r => r.UsersLikes.Count() )
            .MaxAsync( cancellationToken );

        var topLikedRecipes = await _recipeDbContext.Set<Recipe>()
            .Where( r => r.UsersLikes.Count() == maxLikes )
            .ToListAsync( cancellationToken );

        return topLikedRecipes.AsReadOnly();
    }
}
