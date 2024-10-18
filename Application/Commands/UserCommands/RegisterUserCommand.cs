using Application.Models.Result;
using MediatR;

namespace Application.Commands.UserCommands;
public class RegisterUserCommand : IRequest<UserResult>
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string About { get; set; }

    public RegisterUserCommand( string login, string password, string name, string about )
    {
        Login = login;
        Password = password;
        Name = name;
        About = about;
    }
}
