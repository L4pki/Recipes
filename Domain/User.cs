using Domain.RecipeEntities;

namespace Domain;

public class User
{
    public int Id { get; private init; }
    public string Login { get; private set; }
    public string PasswordHash { get; private set; }
    public string Name { get; private set; }
    public string About { get; private set; }

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
