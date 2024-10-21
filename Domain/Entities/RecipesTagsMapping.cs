using Domain.Entities;

namespace Infrastructure.Data;
public class ResipesTagsMapping
{
    public int IdRecipe { get; set; }
    public Recipe Recipe { get; set; }

    public int IdTag { get; set; }
    public Tag Tag { get; set; }
}
