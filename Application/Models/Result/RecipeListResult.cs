using Domain.Entities.RecipeEntities;

namespace Application.Models.Result;
public class RecipeListResult
{
    public IReadOnlyList<Recipe> Recipes { get; set; }
    public string Message { get; set; }

    public RecipeListResult( IReadOnlyList<Recipe> recipes, string message )
    {
        Recipes = recipes;
        Message = message;
    }
}
