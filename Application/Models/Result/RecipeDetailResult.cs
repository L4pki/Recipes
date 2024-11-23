using Domain.Models;

namespace Application.Models.Result;

public class RecipeDetailResult
{
    public RecipeModel Recipe { get; set; }
    public string Message { get; set; }

    public RecipeDetailResult( RecipeModel recipe, string message )
    {
        Recipe = recipe;
        Message = message;
    }
}
