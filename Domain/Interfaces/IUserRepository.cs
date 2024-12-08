using Domain.Entities;
using Domain.Models;

namespace Domain.Interfaces;

public interface IUserRepository
{
    Task CreateAsync( User user, CancellationToken cancellationToken );

    Task<bool> ExistsAsync( string login, CancellationToken cancellationToken );

    Task<User> GetUserByLoginAsync( string login, CancellationToken cancellationToken );

    Task<UserModel> GetUserInfoByIdAsync( int id, CancellationToken cancellationToken );

    Task DeleteAsync( int id, CancellationToken cancellationToken );

    Task<string> StarAsync( int idRecipe, int idUser, CancellationToken cancellationToken );

    Task<User> UpdateAsync( string login, User user, CancellationToken cancellationToken );

    Task<string> UpdateTokenAsync( string refreshToken, int idUser, CancellationToken cancellationToken );
}
