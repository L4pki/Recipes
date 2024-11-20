using Application.Models.Result;
using MediatR;

namespace Application.Commands.UserCommands;

public class LoginUserCommand : IRequest<UserResult>
{
    public string Login { get; set; }
    public string Password { get; set; }

    public LoginUserCommand( string login, string password )
    {
        Login = login;
        Password = password;
    }
}
