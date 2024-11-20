namespace Domain.RecipeEntities;

public class Ingridient
{
    public int Id { get; private init; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public int IdRecipe { get; private init; }

    public Recipe Recipe { get; private init; }

    public Ingridient( string title, string description, int idRecipe )
    {
        Title = title;
        Description = description;
        IdRecipe = idRecipe;
    }

    public Ingridient()
    {
    }
}
