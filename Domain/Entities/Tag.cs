using Infrastructure.Data;

namespace Domain.Entities;
public class Tag
{
    public int Id { get; private init; }
    public string Name { get; private set; }

    public Tag( string name )
    {
        Name = name;
    }

    public List<ResipesTagsMapping> RecipesByTag { get; set; } = new List<ResipesTagsMapping>();
}
