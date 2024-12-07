using Domain.Models;
using MediatR;

namespace Application.Queries;

public class GetUserInfoByIdQuery : IRequest<UserModel>
{
    public int Id { get; set; }

    public GetUserInfoByIdQuery( int id )
    {
        Id = id;
    }
}
