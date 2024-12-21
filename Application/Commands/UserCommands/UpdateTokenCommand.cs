using MediatR;

namespace Application.Commands.UserCommands;

public class UpdateTokenCommand : IRequest<string>
{
    public int IdUser { get; set; }
    public string RefreshToken { get; set; }

    public UpdateTokenCommand( int idUser, string refreshToken )
    {
        IdUser = idUser;
        RefreshToken = refreshToken;
    }
}
