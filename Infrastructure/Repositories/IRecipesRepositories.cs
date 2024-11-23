using Domain.Entities.RecipeEntities;

namespace Domain.Repositories;
public interface IRecipesRepositories
{
    public void Create( Recipe recipes );
    public void Delete( Recipe recipes );
    public Recipe Update( Recipe recipes );
    public Recipe GetByTagsOrName( string tag );
    public Recipe GetAll();
    public Recipe GetByAuthor( string AuthorName );
    public Recipe GetByMostLikes();
}
