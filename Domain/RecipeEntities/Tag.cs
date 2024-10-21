namespace Domain.RecipeEntities;
public class Tag
{
    public int Id { get; private init; }
    public string Name { get; private set; }

    public List<Recipe> RecipesByTag { get; set; } = new List<Recipe>();

    public Tag( string name )
    {
        Name = name;
    }

    public Tag()
    {
    }
}
