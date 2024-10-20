using Domain.Entities;

namespace Domain.Repositories;
public interface IRecipesRepositories
{
    public void Create(Recipes recipes);
    public void Delete(Recipes recipes);
    public Recipes Update(Recipes recipes);
    public Recipes GetByTagsOrName(string tag);
    public Recipes GetAll();
    public Recipes GetByAuthor( string AuthorName );
    public Recipes GetByMostLikes();
}
