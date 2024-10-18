using Application.Commands.UserCommands;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers.UserHandlers.Commands;
public class StarUserCommandHandler : IRequestHandler<StarUserCommand, string>
{
    private readonly IUserRepository _userRepository;

    public StarUserCommandHandler( IUserRepository userRepository )
    {
        _userRepository = userRepository;
    }

    public async Task<string> Handle( StarUserCommand request, CancellationToken cancellationToken )
    {
        return await _userRepository.StarAsync( request.IdRecipe, request.IdUser, cancellationToken );
    }
}
