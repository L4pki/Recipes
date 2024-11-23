using Domain.Entities.RecipeEntities;

namespace Domain.Models;

public class UserModel
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public string Name { get; set; }
    public string About { get; set; }
    public int FavoriteRecipesCount { get; set; }
    public int LikeRecipesCount { get; set; }
    public int PersonalRecipesCount { get; set; }
    public List<Recipe> PersonalRecipes { get; set; }

}
