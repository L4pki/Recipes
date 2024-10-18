using Domain.Entities.RecipeEntities;

namespace Domain.Interfaces.RecipeInterfaces;
public interface ITagRepository
{
    Task CreateAsync( Tag tags, CancellationToken cancellationToken );
    Task<IReadOnlyList<Tag>> GetAllAsync( CancellationToken cancellationToken );
    Task<Tag> GetByNameAsync( string name, CancellationToken cancellationToken );
}
