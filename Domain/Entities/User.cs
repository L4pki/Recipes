using System.Collections.Generic;
using Domain.Entities.Map;

namespace Domain.Entities;

public class User
{
    public int Id { get; private init; }
    public string Login { get; private set; }
    public string HashPassword { get; private set; }
    public string Name { get; private set; }
    public string About { get; private set; }

    public User( string login, string hashPassword )
    {
        Login = login;
        HashPassword = hashPassword;
    }

    public List<Recipe> PersonalRecipes { get; set; } = new List<Recipe>();
    public List<UserRecipeStarMapping> FavoriteRecipes { get; set; } = new List<UserRecipeStarMapping>();
    public List<UserRecipeLikeMapping> LikeRecipes { get; set; } = new List<UserRecipeLikeMapping>();
    public User()
    {
    }
}
