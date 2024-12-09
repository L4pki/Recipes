using Domain.Interfaces;
using Domain.Models;
using MediatR;

namespace Application.Queries.Handlers;
public class GetUserInfoByIdQueryHandler : IRequestHandler<GetUserInfoByIdQuery, UserModel>
{
    private readonly IUserRepository _userRepository;

    public GetUserInfoByIdQueryHandler( IUserRepository userRepository )
    {
        _userRepository = userRepository;
    }

    public async Task<UserModel> Handle( GetUserInfoByIdQuery request, CancellationToken cancellationToken )
    {
        return await _userRepository.GetUserInfoByIdAsync( request.Id, cancellationToken );
    }
}
