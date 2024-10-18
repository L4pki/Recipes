using Domain.Entities;
using MediatR;

namespace Application.Querys;
public class GetUserByLoginQuery : IRequest<User>
{
    public string Login { get; set; }

    public GetUserByLoginQuery( string login )
    {
        Login = login;
    }
}
