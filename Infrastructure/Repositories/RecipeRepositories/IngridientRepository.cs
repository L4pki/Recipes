using Domain.Entities.RecipeEntities;
using Domain.Interfaces.RecipeInterfaces;
using Infrastructure.Data;

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
}
