namespace Domain.Entities.RecipeEntities;

public class Ingridient
{
    public int Id { get; init; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int IdRecipe { get; init; }

    public Recipe Recipe { get; set; }

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
