using Infrastructure.Data;

namespace Domain.Entities;

public class Recipe
{
    public int Id { get; private init; }
    public string Name { get; private set; }
    public string ShortDescription { get; private set; }
    public string PhotoURL { get; private set; }
    public int IdAuthor { get; private set; }
    public TimeSpan TimeCosts { get; private set; }
    public int NumberOfPersons { get; private set; }
    public int Likes { get; private set; }
    public int Stars { get; private set; }

    public Recipe(
        string name,
        string shortDescription,
        string photoUrl,
        int idAuthor,
        TimeSpan timeCosts,
        int numberOfPersons )
    {
        Name = name;
        ShortDescription = shortDescription;
        PhotoURL = photoUrl;
        IdAuthor = idAuthor;
        TimeCosts = timeCosts;
        NumberOfPersons = numberOfPersons;
    }

    public List<RecipesTagsMapping> Tags { get; private set; } = new List<RecipesTagsMapping>();
    public List<Step> StepOfCooking { get; private set; } = new List<Step>();
    public List<Ingridient> IngridientForCooking { get; private set; } = new List<Ingridient>();
    public List<UserRecipeLikeMapping> UsersLikes { get; private set; } = new List<UserRecipeLikeMapping>();
    public List<UserRecipeStarMapping> UsersStars { get; private set; } = new List<UserRecipeStarMapping>();
    public User Author { get; private set; }

}
