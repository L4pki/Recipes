namespace Application.Models.Result;

public class RecipeStatusResult
{
    public bool RecipeLiked { get; set; }
    public bool RecipeStarred { get; set; }

    public RecipeStatusResult( bool recipeLiked, bool recipeStarred )
    {
        RecipeLiked = recipeLiked;
        RecipeStarred = recipeStarred;
    }
}
