using Domain.Entities.RecipeEntities;

namespace Domain.Interfaces.RecipeInterfaces;
public interface IStepRepository
{
    Task CreateAsync( Step step, CancellationToken cancellationToken );
}
