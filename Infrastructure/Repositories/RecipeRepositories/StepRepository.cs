using Domain.Entities.RecipeEntities;
using Domain.Interfaces.RecipeInterfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

    public async Task DeleteAsync( Step step, CancellationToken cancellationToken )
    {
        if ( step == null )
        {
            throw new ArgumentNullException( nameof( step ) );
        }

        _recipeDbContext.Set<Step>().Remove( step );
        await _recipeDbContext.SaveChangesAsync( cancellationToken );
    }

    public async Task<List<Step>> GetByRecipeIdAsync( int recipeId, CancellationToken cancellationToken )
    {
        return await _recipeDbContext.Set<Step>()
            .Where( i => i.IdRecipe == recipeId )
            .ToListAsync( cancellationToken );
    }
}
