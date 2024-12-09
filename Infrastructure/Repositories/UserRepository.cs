using Domain.Entities;
using Domain.Entities.RecipeEntities;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly RecipeDbContext _recipeDbContext;

    public UserRepository( RecipeDbContext recipeDbContext )
    {
        _recipeDbContext = recipeDbContext;
    }

    public async Task CreateAsync( User user, CancellationToken cancellationToken )
    {
        await _recipeDbContext.AddAsync( user, cancellationToken );
        await _recipeDbContext.SaveChangesAsync( cancellationToken );
    }

    public async Task<bool> ExistsAsync( string login, CancellationToken cancellationToken )
    {
        return await _recipeDbContext.Set<User>().AnyAsync( u => u.Login == login, cancellationToken );
    }

    public async Task<User> GetUserByLoginAsync( string login, CancellationToken cancellationToken )
    {
        return await _recipeDbContext.Set<User>().FirstOrDefaultAsync( u => u.Login == login, cancellationToken );

    }

    public async Task DeleteAsync( int id, CancellationToken cancellationToken )
    {
        var user = await _recipeDbContext.Set<User>().FirstOrDefaultAsync( u => u.Id == id, cancellationToken );
        if ( user != null )
        {
            _recipeDbContext.Remove( user );
            await _recipeDbContext.SaveChangesAsync( cancellationToken );
        }
    }

    public async Task<UserModel> GetUserInfoByIdAsync( int id, CancellationToken cancellationToken )
    {
        var userModel = await _recipeDbContext.Set<User>()
        .Where( u => u.Id == id )
        .Include( u => u.PersonalRecipes )
        .Include( u => u.FavoriteRecipes )
        .Include( u => u.LikeRecipes )
        .Select( u => new UserModel
        {
            Id = u.Id,
            Login = u.Login,
            Name = u.Name,
            About = u.About,
            FavoriteRecipesCount = u.FavoriteRecipes.Count,
            LikeRecipesCount = u.LikeRecipes.Count,
            PersonalRecipesCount = u.PersonalRecipes.Count,
            PersonalRecipes = u.PersonalRecipes.ToList()
        } )
        .FirstOrDefaultAsync( cancellationToken );

        return userModel;
    }

    public async Task<User> UpdateAsync( string login, User user, CancellationToken cancellationToken )
    {
        var existingUser = await _recipeDbContext.Set<User>().FirstOrDefaultAsync( u => u.Login == login, cancellationToken );
        existingUser.Name = user.Name;
        existingUser.About = user.About;
        existingUser.PasswordHash = user.PasswordHash;
        await _recipeDbContext.SaveChangesAsync( cancellationToken );
        return existingUser;
    }

    public async Task<string> StarAsync( int idRecipe, int idUser, CancellationToken cancellationToken )
    {
        var user = await _recipeDbContext.Set<User>()
            .Include( u => u.FavoriteRecipes )
            .FirstOrDefaultAsync( u => u.Id == idUser, cancellationToken );

        if ( user != null )
        {
            var recipe = await _recipeDbContext.Set<Recipe>().FindAsync( idRecipe );
            if ( recipe != null )
            {
                if ( user.FavoriteRecipes.Any( r => r.Id == idRecipe ) )
                {
                    user.FavoriteRecipes.Remove( recipe );
                    await _recipeDbContext.SaveChangesAsync( cancellationToken );
                    return "Рецепт удален из избранного";
                }
                else
                {
                    user.FavoriteRecipes.Add( recipe );
                    await _recipeDbContext.SaveChangesAsync( cancellationToken );
                    return "Рецепт добавлен в избранное";
                }
            }
            return "Рецепт не найден";
        }
        return "Пользователь не найден";
    }

    public async Task<string> UpdateTokenAsync( string refreshToken, int idUser, CancellationToken cancellationToken )
    {
        User user = await _recipeDbContext.Set<User>()
            .FirstOrDefaultAsync( u => u.Id == idUser, cancellationToken );
        if ( user != null )
        {
            user.RefreshToken = refreshToken;
            await _recipeDbContext.SaveChangesAsync( cancellationToken );
            return "Токен обновлен";
        }
        return "Пользователь не найден";
    }
}
