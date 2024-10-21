namespace Domain.Entities.Map;
public class UserRecipeLikeMapping
{
    public int IdUser { get; private set; }
    public User User { get; private set; }

    public int IdRecipe { get; private set; }
    public Recipe Recipe { get; private set; }

    public UserRecipeLikeMapping()
    {
    }
}
