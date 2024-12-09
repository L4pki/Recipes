using Domain.Entities.RecipeEntities;

namespace Domain.Interfaces.RecipeInterfaces;
public interface IStepRepository
{
    Task CreateAsync( Step step, CancellationToken cancellationToken );

    Task DeleteAsync( Step step, CancellationToken cancellationToken );

    Task<List<Step>> GetByRecipeIdAsync( int id, CancellationToken cancellationToken );
}
