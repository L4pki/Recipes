namespace Domain.Entities.RecipeEntities;

public class Step
{
    public int Id { get; init; }
    public string Description { get; set; }
    public int IdRecipe { get; init; }
    public int NumberOfStep { get; set; }

    public Recipe Recipe { get; set; }

    public Step( int idRecipe, string description )
    {
        IdRecipe = idRecipe;
        Description = description;
    }

    public Step()
    {
    }
}
