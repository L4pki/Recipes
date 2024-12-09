using Domain.Entities.RecipeEntities;
using Domain.Interfaces.RecipeInterfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.RecipeRepositories;
public class TagRepository : ITagRepository
{
    private readonly RecipeDbContext _recipeDbContext;

    public TagRepository( RecipeDbContext recipeDbContext )
    {
        _recipeDbContext = recipeDbContext;
    }

    public async Task CreateAsync( Tag tag, CancellationToken cancellationToken )
    {
        bool tagExists = await _recipeDbContext.Set<Tag>()
            .AnyAsync( t => t.Name.ToLower() == tag.Name.ToLower(), cancellationToken );

        if ( tagExists )
        {
            throw new InvalidOperationException( "Тег с таким названием уже существует." );
        }

        await _recipeDbContext.Set<Tag>().AddAsync( tag, cancellationToken );
        await _recipeDbContext.SaveChangesAsync( cancellationToken );
    }

    public async Task<IReadOnlyList<Tag>> GetAllAsync( CancellationToken cancellationToken )
    {
        var tags = await _recipeDbContext.Tags.ToListAsync( cancellationToken );
        return tags;
    }

    public async Task<Tag> GetByNameAsync( string name, CancellationToken cancellationToken )
    {
        return await _recipeDbContext.Set<Tag>().FirstOrDefaultAsync( u => u.Name == name, cancellationToken );
    }

    public async Task<IReadOnlyList<Tag>> GetPopularTagAsync( CancellationToken cancellationToken )
    {
        return await _recipeDbContext.Tags
            .Where( tag => tag.RecipesByTag.Any() ) 
            .GroupBy( tag => tag.Id )
            .OrderByDescending( g => g.Count() ) 
            .Select( g => new
            {
                Tag = g.FirstOrDefault(), 
                Count = g.Count() 
            } )
            .Take( 10 ) 
            .ToListAsync( cancellationToken ) 
            .ContinueWith( task => task.Result.Select( x => x.Tag ).ToList(), cancellationToken );
    }
}
