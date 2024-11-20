using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserRepository
{
    Task CreateAsync( User user, CancellationToken cancellationToken );

    Task<bool> ExistsAsync( string login, CancellationToken cancellationToken );

    Task<User> GetUserInfoByLoginAsync( string login, CancellationToken cancellationToken );

    Task<User> GetUserByIdAsync( int id, CancellationToken cancellationToken );

    Task DeleteAsync( int id, CancellationToken cancellationToken );

    Task<string> StarAsync( int idRecipe, int idUser, CancellationToken cancellationToken );

    Task<User> UpdateAsync( string login, User user, CancellationToken cancellationToken );
}
