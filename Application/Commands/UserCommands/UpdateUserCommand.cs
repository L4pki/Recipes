using Application.Models.Result;
using MediatR;

namespace Application.Commands.UserCommands;
public class UpdateUserCommand : IRequest<UserResult>
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string About { get; set; }
    public string FormerLogin { get; set; }

    public UpdateUserCommand( string login, string password, string name, string about, string formerLogin )
    {
        Login = login;
        Password = password;
        Name = name;
        About = about;
        FormerLogin = formerLogin;
    }
}
