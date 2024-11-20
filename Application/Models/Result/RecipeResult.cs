using Domain.Entities.RecipeEntities;

namespace Application.Models.Result;

public class RecipeResult
{
    public Recipe Recipe { get; set; }
    public string Message { get; set; }

    public RecipeResult( Recipe recipe, string message )
    {
        Recipe = recipe;
        Message = message;
    }
}
