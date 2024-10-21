namespace Domain.RecipeEntities;

public class Recipe
{
    public int Id { get; private init; }
    public string Name { get; private set; }
    public string ShortDescription { get; private set; }
    public string PhotoUrl { get; private set; }
    public int IdAuthor { get; private set; }
    public TimeSpan TimeCosts { get; private set; }
    public int NumberOfPersons { get; private set; }

    public User Author { get; private set; }

    public List<Step> StepOfCooking { get; private set; } = new List<Step>();
    public List<Ingridient> IngridientForCooking { get; private set; } = new List<Ingridient>();
    public List<Tag> Tags { get; private set; } = new List<Tag>();
    public List<User> UsersLikes { get; private set; } = new List<User>();
    public List<User> UsersStars { get; private set; } = new List<User>();

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
        PhotoUrl = photoUrl;
        IdAuthor = idAuthor;
        TimeCosts = timeCosts;
        NumberOfPersons = numberOfPersons;
    }

    public Recipe()
    {
    }

}
