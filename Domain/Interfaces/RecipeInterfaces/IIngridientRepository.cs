using Domain.Entities.RecipeEntities;

namespace Domain.Interfaces.RecipeInterfaces;
public interface IIngridientRepository
{
    Task CreateAsync( Ingridient ingridient, CancellationToken cancellationToken );
}
