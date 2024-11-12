using Application.Models.Dto;

namespace WebAPI.Dto.RecipeDto;

public class UpdateRecipeDto
{
    public int IdRecipe { get; set; }
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string PhotoUrl { get; set; }
    public TimeSpan TimeCosts { get; set; }
    public int NumberOfPersons { get; set; }
    public List<IngridientDto> Ingridients { get; set; }
    public List<StepDto> Steps { get; set; }
    public List<TagDto> Tags { get; set; }

}
