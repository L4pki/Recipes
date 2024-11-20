using MediatR;

namespace Application.Queries;

public class CheckUserQuery : IRequest<bool>
{
    public string Login { get; set; }

    public CheckUserQuery( string login )
    {
        Login = login;
    }
}
