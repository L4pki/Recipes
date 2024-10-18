using MediatR;

namespace Application.Querys;
public class CheckUserQuery : IRequest<bool>
{
    public string Login { get; set; }

    public CheckUserQuery( string login )
    {
        Login = login;
    }
}
