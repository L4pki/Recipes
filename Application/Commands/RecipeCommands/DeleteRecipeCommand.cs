using MediatR;

namespace Application.Commands.RecipeCommands;
public class DeleteRecipeCommand : IRequest<string>
{
    public int Id { get; set; }
    public int IdAuthor { get; set; }

    public DeleteRecipeCommand(
        int idRecipe,
        int idAuthor )
    {
        Id = idRecipe;
        IdAuthor = idAuthor;
    }
}
