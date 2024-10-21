namespace Domain.Entities;
public class UserRecipeStarMapping
{
    public int IdUser { get; private set; }
    public User User { get; private set; }

    public int IdRecipe { get; private set; }
    public Recipe Recipe { get; private set; }
}
