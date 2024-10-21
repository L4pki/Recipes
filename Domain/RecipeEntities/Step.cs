namespace Domain.RecipeEntities;
public class Step
{
    public int Id { get; private init; }
    public string Description { get; private set; }
    public int IdRecipe { get; private init; }
    public int NumberOfStep { get; private set; }

    public Recipe Recipe { get; private set; }

    public Step( int idRecipe, string description, int numberOfStep )
    {
        IdRecipe = idRecipe;
        Description = description;
        NumberOfStep = numberOfStep;
    }

    public Step()
    {
    }
}
