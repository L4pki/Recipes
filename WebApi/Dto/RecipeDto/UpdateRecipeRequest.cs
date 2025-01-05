namespace WebAPI.Dto.RecipeDto;

public class UpdateRecipeRequest
{
    public UpdateRecipeDto Recipe { get; set; }
    public IFormFile Image { get; set; }
}
