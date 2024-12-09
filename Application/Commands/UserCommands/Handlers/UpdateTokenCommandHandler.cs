using Domain.Interfaces;
using MediatR;

namespace Application.Commands.UserCommands.Handlers;

public class UpdateTokenCommandHandler : IRequestHandler<UpdateTokenCommand, string>
{
    private readonly IUserRepository _userRepository;

    public UpdateTokenCommandHandler( IUserRepository userRepository )
    {
        _userRepository = userRepository;
    }

    public async Task<string> Handle( UpdateTokenCommand request, CancellationToken cancellationToken )
    {

        if ( string.IsNullOrWhiteSpace( request.RefreshToken ) )
        {
            return ( "Ошибка: Токен не сгенерирован!" );
        }

        string answer = await _userRepository.UpdateTokenAsync( request.RefreshToken, request.IdUser, cancellationToken );
        return ( answer );
    }
}
