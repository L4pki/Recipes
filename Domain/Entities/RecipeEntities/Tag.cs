namespace Domain.Entities.RecipeEntities;

public class Tag
{
    public int Id { get; init; }
    public string Name { get; set; }

    public List<Recipe> RecipesByTag { get; set; } = new List<Recipe>();

    public Tag( string name )
    {
        Name = name;
    }

    public Tag()
    {
    }
}
