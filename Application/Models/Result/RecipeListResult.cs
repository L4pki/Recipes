using Domain.Entities.RecipeEntities;
using Domain.Models;

namespace Application.Models.Result;

public class RecipeListResult
{
    public IReadOnlyList<RecipeModel> Recipes { get; set; }
    public string Message { get; set; }

    public RecipeListResult( IReadOnlyList<RecipeModel> recipes, string message )
    {
        Recipes = recipes;
        Message = message;
    }
}
