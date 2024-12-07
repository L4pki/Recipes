using Application.Models.Result;
using Application.Queries;
using Application.Seсurity;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.UserCommands.Handlers;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMediator _mediator;

    public RegisterUserCommandHandler( IUserRepository userRepository, IPasswordHasher passwordHasher, IMediator mediator )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _mediator = mediator;
    }

    public async Task<UserResult> Handle( RegisterUserCommand request, CancellationToken cancellationToken )
    {
        if ( string.IsNullOrWhiteSpace( request.Login ) || string.IsNullOrWhiteSpace( request.Password ) || string.IsNullOrWhiteSpace( request.Name ) )
        {
            return new UserResult( null, "Ошибка: Все поля обязательны для заполнения." );
        }

        if ( await _mediator.Send( new CheckUserQuery( request.Login ), cancellationToken ) )
        {
            return new UserResult( null, "Ошибка: Логин уже существует." );
        }

        if ( request.Password.Length < 6 )
        {
            return new UserResult( null, "Ошибка: Пароль должен содержать не менее 6 символов." );
        }

        string hashedPassword = _passwordHasher.HashPassword( request.Password );

        User newUser = new User
        {
            Login = request.Login,
            PasswordHash = hashedPassword,
            Name = request.Name,
            About = request.About,
            RefreshToken = request.RefreshToken,
        };

        await _userRepository.CreateAsync( newUser, cancellationToken );
        User user = await _mediator.Send( new GetUserByLoginQuery( request.Login ), cancellationToken );

        return new UserResult( user, "Регистрация прошла успешно!" );
    }
}
