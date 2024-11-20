using Application.Models.Result;
using Application.Querys;
using Application.Seсurity;
using Domain.Entities;
using MediatR;

namespace Application.Commands.UserCommands.Handlers;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, UserResult>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMediator _mediator;

    public LoginUserCommandHandler( IPasswordHasher passwordHasher, IMediator mediator )
    {
        _passwordHasher = passwordHasher;
        _mediator = mediator;
    }

    public async Task<UserResult> Handle( LoginUserCommand request, CancellationToken cancellationToken )
    {

        if ( string.IsNullOrWhiteSpace( request.Login ) || string.IsNullOrWhiteSpace( request.Password ) )
        {
            return new UserResult( null, "Ошибка: Все поля обязательны для заполнения." );
        }

        if ( !await _mediator.Send( new CheckUserQuery( request.Login ), cancellationToken ) )
        {
            return new UserResult( null, "Ошибка: Логин или пароль не совпадают." );
        }

        User user = await _mediator.Send( new GetUserByLoginQuery( request.Login ), cancellationToken );

        if ( !( _passwordHasher.HashPassword( request.Password ) == user.PasswordHash ) )
        {
            return new UserResult( null, "Ошибка: Логин или пароль не совпадают." );
        }

        return new UserResult( user, "Вход прошел успешно!" );
    }
}
