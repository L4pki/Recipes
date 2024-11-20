using MediatR;

namespace Application.Commands.RecipeCommands;

public class LikeRecipeCommand : IRequest<string>
{
    public int IdRecipe { get; set; }
    public int IdUser { get; set; }

    public LikeRecipeCommand(
        int idRecipe,
        int idUser )
    {
        IdRecipe = idRecipe;
        IdUser = idUser;
    }
}
