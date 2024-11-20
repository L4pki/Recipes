using Domain.Entities.RecipeEntities;
using Domain.Interfaces.RecipeInterfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories.RecipeRepositories;
public class StepRepository : IStepRepository
{
    private readonly RecipeDbContext _recipeDbContext;

    public StepRepository( RecipeDbContext recipeDbContext )
    {
        _recipeDbContext = recipeDbContext;
    }

    public async Task CreateAsync( Step step, CancellationToken cancellationToken )
    {
        await _recipeDbContext.Set<Step>().AddAsync( step, cancellationToken );
        await _recipeDbContext.SaveChangesAsync( cancellationToken );
    }
}
