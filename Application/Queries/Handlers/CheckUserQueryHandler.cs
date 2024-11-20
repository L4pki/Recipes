using Domain.Interfaces;
using MediatR;

namespace Application.Queries.Handlers;

public class CheckUserQueryHandler : IRequestHandler<CheckUserQuery, bool>
{
    private readonly IUserRepository _userRepository;

    public CheckUserQueryHandler( IUserRepository userRepository )
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle( CheckUserQuery request, CancellationToken cancellationToken )
    {
        return await _userRepository.ExistsAsync( request.Login, cancellationToken );
    }
}
