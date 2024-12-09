using Domain.Entities.RecipeEntities;

namespace Application.Models.Result;
public class TagListResult
{
    public IReadOnlyList<Tag> Tags { get; set; }

    public TagListResult( IReadOnlyList<Tag> tags )
    {
        Tags = tags;
    }
}
