using Domain.Entities;

namespace Domain.Entities.Map;
public class RecipesTagsMapping
{
    public int IdRecipe { get; set; }
    public Recipe Recipe { get; set; }

    public int IdTag { get; set; }
    public Tag Tag { get; set; }

    public RecipesTagsMapping()
    {
    }
}
