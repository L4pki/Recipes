namespace Domain.Entities;
public class Tag
{
    public int Id { get; private init; }
    public string Name { get; private set; }

    public Tag( string name )
    {
        Name = name;
    }

    public List<Recipe> RecipesByTag { get; set; } = new List<Recipe>();
}
