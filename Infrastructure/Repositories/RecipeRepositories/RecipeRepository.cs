using Domain.Entities;
using Domain.Entities.RecipeEntities;
using Domain.Interfaces.RecipeInterfaces;
using Domain.Models;
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
        recipe.IdAuthor = modifiedRecipe.IdAuthor;
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

    public async Task<RecipeModel> GetDetailByIdAsync( int id, CancellationToken cancellationToken )
    {
        var recipe = await _recipeDbContext.Set<Recipe>()
            .Where( r => r.Id == id )
            .Include( r => r.Tags )
            .Include( r => r.IngridientForCooking )
            .Include( r => r.StepOfCooking )
            .Include( r => r.UsersLikes )
            .Include( r => r.UsersStars )
            .Select( r => new RecipeModel
            {
                Id = r.Id,
                Name = r.Name,
                ShortDescription = r.ShortDescription,
                PhotoUrl = r.PhotoUrl,
                IdAuthor = r.IdAuthor,
                TimeCosts = r.TimeCosts,
                Tags = r.Tags.ToList(),
                IngridientForCooking = r.IngridientForCooking.ToList(),
                StepOfCooking = r.StepOfCooking.ToList(),
                NumberOfPersons = r.NumberOfPersons,
                UsersLikesCount = r.UsersLikes.Count,
                UsersStarsCount = r.UsersStars.Count
            } )
            .FirstOrDefaultAsync( cancellationToken );

        return recipe;
    }

    public async Task<IReadOnlyList<bool>> RecipeLikeStarStatus( int userId, int recipeId, CancellationToken cancellationToken )
    {
        var recipe = await _recipeDbContext.Set<Recipe>()
            .Include( r => r.UsersLikes )
            .Include( r => r.UsersStars )
            .FirstOrDefaultAsync( r => r.Id == recipeId, cancellationToken );

        if ( recipe != null )
        {
            var user = await _recipeDbContext.Set<User>().FindAsync( userId );
            if ( user != null )
            {
                bool isLiked = recipe.UsersLikes.Any( u => u.Id == userId );
                bool isStarred = recipe.UsersStars.Any( u => u.Id == userId );

                return new List<bool> { isLiked, isStarred };
            }
            else
            {
                throw new KeyNotFoundException( "Пользователь не найден" );
            }
        }
        else
        {
            throw new KeyNotFoundException( "Рецепт не найден" );
        }
    }


    public async Task<IReadOnlyList<RecipeModel>> GetByTagsOrNameAsync( string searchQuery, CancellationToken cancellationToken )
    {
        if ( string.IsNullOrWhiteSpace( searchQuery ) )
        {
            return new List<RecipeModel>().AsReadOnly();
        }

        var recipes = await _recipeDbContext.Set<Recipe>()
            .Include( r => r.Tags )
            .Where( r => r.Name.ToLower().Contains( searchQuery.ToLower() ) ||
                r.Tags.Any( t => t.Name.ToLower().Contains( searchQuery.ToLower() ) ) )
            .Include( r => r.Tags )
            .Include( r => r.UsersLikes )
            .Include( r => r.UsersStars )
            .Select( r => new RecipeModel
            {
                Id = r.Id,
                Name = r.Name,
                ShortDescription = r.ShortDescription,
                PhotoUrl = r.PhotoUrl,
                IdAuthor = r.IdAuthor,
                TimeCosts = r.TimeCosts,
                Tags = r.Tags,
                NumberOfPersons = r.NumberOfPersons,
                UsersLikesCount = r.UsersLikes.Count,
                UsersStarsCount = r.UsersStars.Count
            } )
            .ToListAsync( cancellationToken );

        return recipes.AsReadOnly();
    }

    public async Task<IReadOnlyList<RecipeModel>> GetFavoriteRecipesAsync( int userId, CancellationToken cancellationToken )
    {
        var recipes = await _recipeDbContext.Users
            .Where( u => u.Id == userId )
            .SelectMany( u => u.FavoriteRecipes )
            .Include( r => r.Tags )
            .Include( r => r.UsersLikes )
            .Include( r => r.UsersStars )
            .Select( r => new RecipeModel
            {
                Id = r.Id,
                Name = r.Name,
                ShortDescription = r.ShortDescription,
                PhotoUrl = r.PhotoUrl,
                IdAuthor = r.IdAuthor,
                TimeCosts = r.TimeCosts,
                Tags = r.Tags.ToList(),
                NumberOfPersons = r.NumberOfPersons,
                UsersLikesCount = r.UsersLikes.Count,
                UsersStarsCount = r.UsersStars.Count
            } )
            .ToListAsync( cancellationToken );

        foreach ( var recipe in recipes )
        {
            Console.WriteLine( $"Recipe: {recipe.Name}, Tags: {string.Join( ", ", recipe.Tags.Select( t => t.Name ) )}" );
        }

        return recipes.AsReadOnly();
    }

    public async Task<IReadOnlyList<RecipeModel>> GetLikedRecipesAsync( int userId, CancellationToken cancellationToken )
    {
        var recipes = await _recipeDbContext.Users
            .Where( u => u.Id == userId )
            .SelectMany( u => u.LikeRecipes )
            .Include( r => r.Tags )
            .Include( r => r.UsersLikes )
            .Include( r => r.UsersStars )
            .Select( r => new RecipeModel
            {
                Id = r.Id,
                Name = r.Name,
                ShortDescription = r.ShortDescription,
                PhotoUrl = r.PhotoUrl,
                IdAuthor = r.IdAuthor,
                TimeCosts = r.TimeCosts,
                Tags = r.Tags,
                NumberOfPersons = r.NumberOfPersons,
                UsersLikesCount = r.UsersLikes.Count,
                UsersStarsCount = r.UsersStars.Count
            } )
            .ToListAsync( cancellationToken );
        return recipes.AsReadOnly();
    }

    public async Task<IReadOnlyList<RecipeModel>> GetByAuthorAsync( int idAuthor, CancellationToken cancellationToken )
    {
        var recipes = await _recipeDbContext.Set<Recipe>()
            .Where( r => r.IdAuthor == idAuthor )
            .Include( r => r.Tags )
            .Include( r => r.UsersLikes )
            .Include( r => r.UsersStars )
            .Select( r => new RecipeModel
            {
                Id = r.Id,
                Name = r.Name,
                ShortDescription = r.ShortDescription,
                PhotoUrl = r.PhotoUrl,
                IdAuthor = r.IdAuthor,
                TimeCosts = r.TimeCosts,
                Tags = r.Tags,
                NumberOfPersons = r.NumberOfPersons,
                UsersLikesCount = r.UsersLikes.Count,
                UsersStarsCount = r.UsersStars.Count
            } )
                                    .ToListAsync( cancellationToken );
        return recipes.AsReadOnly();
    }

    public async Task<IReadOnlyList<RecipeModel>> GetAllAsync( CancellationToken cancellationToken )
    {
        var recipes = await _recipeDbContext.Set<Recipe>()
            .Include( r => r.Tags )
            .Include( r => r.UsersLikes )
            .Include( r => r.UsersStars )
            .Select( r => new RecipeModel
            {
                Id = r.Id,
                Name = r.Name,
                ShortDescription = r.ShortDescription,
                PhotoUrl = r.PhotoUrl,
                IdAuthor = r.IdAuthor,
                TimeCosts = r.TimeCosts,
                Tags = r.Tags,
                NumberOfPersons = r.NumberOfPersons,
                UsersLikesCount = r.UsersLikes.Count,
                UsersStarsCount = r.UsersStars.Count
            } )
            .ToListAsync( cancellationToken );

        return recipes.AsReadOnly();
    }

    public async Task<IReadOnlyList<RecipeModel>> GetMostLikedRecipesAsync( CancellationToken cancellationToken )
    {
        var recipeLikes = await _recipeDbContext.Set<Recipe>()
            .Select( r => new
            {
                Recipe = r,
                LikesCount = r.UsersLikes.Count(),
                StarsCount = r.UsersStars.Count()
            } )
            .ToListAsync( cancellationToken );
        var maxLikes = recipeLikes.Max( x => x.LikesCount );
        var topLikedRecipes = recipeLikes
            .Where( x => x.LikesCount == maxLikes )
            .Select( x => new RecipeModel
            {
                Id = x.Recipe.Id,
                Name = x.Recipe.Name,
                ShortDescription = x.Recipe.ShortDescription,
                PhotoUrl = x.Recipe.PhotoUrl,
                IdAuthor = x.Recipe.IdAuthor,
                TimeCosts = x.Recipe.TimeCosts,
                Tags = x.Recipe.Tags,
                NumberOfPersons = x.Recipe.NumberOfPersons,
                UsersLikesCount = x.LikesCount,
                UsersStarsCount = x.StarsCount
            } )
            .ToList();

        return topLikedRecipes.AsReadOnly();
    }
}
