using Application.Querys;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Querys.Handlers;
public class GetUserByLoginQueryHandler : IRequestHandler<GetUserByLoginQuery, User>
{
    private readonly IUserRepository _userRepository;

    public GetUserByLoginQueryHandler( IUserRepository userRepository )
    {
        _userRepository = userRepository;
    }

    public async Task<User> Handle( GetUserByLoginQuery request, CancellationToken cancellationToken )
    {
        return await _userRepository.GetUserInfoByLoginAsync( request.Login, cancellationToken );
    }
}
