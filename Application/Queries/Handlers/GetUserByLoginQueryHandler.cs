using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.Handlers;

public class GetUserByLoginQueryHandler : IRequestHandler<GetUserByLoginQuery, User>
{
    private readonly IUserRepository _userRepository;

    public GetUserByLoginQueryHandler( IUserRepository userRepository )
    {
        _userRepository = userRepository;
    }

    public async Task<User> Handle( GetUserByLoginQuery request, CancellationToken cancellationToken )
    {
        return await _userRepository.GetUserByLoginAsync( request.Login, cancellationToken );
    }
}
