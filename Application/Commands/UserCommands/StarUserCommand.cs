using MediatR;

namespace Application.Commands.UserCommands;
public class StarUserCommand : IRequest<string>
{
    public int IdRecipe { get; set; }
    public int IdUser { get; set; }

    public StarUserCommand(
        int idRecipe,
        int idUser )
    {
        IdRecipe = idRecipe;
        IdUser = idUser;
    }
}
