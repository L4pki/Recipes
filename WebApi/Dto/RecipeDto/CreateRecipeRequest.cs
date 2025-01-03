namespace WebAPI.Dto.RecipeDto;

public class CreateRecipeRequest
{
    public CreateRecipeDto Recipe { get; set; }
    public IFormFile Image { get; set; }
}
