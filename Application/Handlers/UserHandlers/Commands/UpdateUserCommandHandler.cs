using Application.Commands.UserCommands;
using Application.Models.Result;
using Application.Sequrity;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Handlers.UserHandlers.Commands;
public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserResult>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler( IUserRepository userRepository, IPasswordHasher passwordHasher )
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
    }

    public async Task<UserResult> Handle( UpdateUserCommand request, CancellationToken cancellationToken )
    {

        if ( string.IsNullOrWhiteSpace( request.Login ) ||
            string.IsNullOrWhiteSpace( request.Password ) ||
            string.IsNullOrWhiteSpace( request.Name ) ||
            string.IsNullOrWhiteSpace( request.About ) )
        {
            return new UserResult( null, "Ошибка: Все поля обязательны для заполнения." );
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
            About = request.About
        };

        User existingUser = await _userRepository.UpdateAsync( request.FormerLogin, newUser, cancellationToken );
        return new UserResult( existingUser, "Данные изменены успешно!" );
    }
}
