using Domain.Entities.RecipeEntities;

namespace Domain.Interfaces.RecipeInterfaces;
public interface IIngridientRepository
{
    Task CreateAsync( Ingridient ingridient, CancellationToken cancellationToken );

    Task DeleteAsync( Ingridient ingridient, CancellationToken cancellationToken );

    Task<List<Ingridient>> GetByRecipeIdAsync( int id, CancellationToken cancellationToken );
}
