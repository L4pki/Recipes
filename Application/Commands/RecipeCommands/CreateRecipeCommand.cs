using Application.Models.Dto;
using Application.Models.Result;
using MediatR;

namespace Application.Commands.RecipeCommands;

public class CreateRecipeCommand : IRequest<RecipeResult>
{
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string PhotoUrl { get; set; }
    public int IdAuthor { get; set; }
    public string AuthorName { get; set; }
    public int TimeCosts { get; set; }
    public int NumberOfPersons { get; set; }
    public IngridientDto[] Ingridients { get; set; }
    public StepDto[] Steps { get; set; }
    public TagDto[] Tags { get; set; }

    public CreateRecipeCommand(
        string name,
        string shortDescription,
        string photoUrl,
        int idAuthor,
        string authorName,
        int timeCosts,
        int numberOfPersons,
        IngridientDto[] ingridients,
        StepDto[] steps,
        TagDto[] tags )
    {
        Name = name;
        ShortDescription = shortDescription;
        PhotoUrl = photoUrl;
        IdAuthor = idAuthor;
        AuthorName = authorName;
        TimeCosts = timeCosts;
        NumberOfPersons = numberOfPersons;
        Ingridients = ingridients;
        Steps = steps;
        Tags = tags;
    }
}
