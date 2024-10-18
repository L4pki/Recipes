using Domain.Entities.RecipeEntities;

namespace Domain.Entities;

public class User
{
    public int Id { get; private init; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public string Name { get; set; }
    public string About { get; set; }

    public List<Recipe> PersonalRecipes { get; private set; } = new List<Recipe>();
    public List<Recipe> FavoriteRecipes { get; set; } = new List<Recipe>();
    public List<Recipe> LikeRecipes { get; set; } = new List<Recipe>();

    public User( string login, string passwordHash )
    {
        Login = login;
        PasswordHash = passwordHash;
    }

    public User()
    {
    }
}
