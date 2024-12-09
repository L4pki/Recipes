using Domain.Entities.RecipeEntities;
using Domain.Interfaces.RecipeInterfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.RecipeRepositories;
public class IngridientRepository : IIngridientRepository
{
    private readonly RecipeDbContext _recipeDbContext;

    public IngridientRepository( RecipeDbContext recipeDbContext )
    {
        _recipeDbContext = recipeDbContext;
    }

    public async Task CreateAsync( Ingridient ingridient, CancellationToken cancellationToken )
    {
        await _recipeDbContext.Set<Ingridient>().AddAsync( ingridient, cancellationToken );
        await _recipeDbContext.SaveChangesAsync( cancellationToken );
    }

    public async Task DeleteAsync( Ingridient ingridient, CancellationToken cancellationToken )
    {
        if ( ingridient == null )
        {
            throw new ArgumentNullException( nameof( ingridient ) );
        }

        _recipeDbContext.Set<Ingridient>().Remove( ingridient );
        await _recipeDbContext.SaveChangesAsync( cancellationToken );
    }

    public async Task<List<Ingridient>> GetByRecipeIdAsync( int recipeId, CancellationToken cancellationToken )
    {
        return await _recipeDbContext.Set<Ingridient>()
            .Where( i => i.IdRecipe == recipeId )
            .ToListAsync( cancellationToken );
    }
}
