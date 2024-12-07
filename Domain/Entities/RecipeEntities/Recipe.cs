namespace Domain.Entities.RecipeEntities;

public class Recipe
{
    public int Id { get; init; }
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string PhotoUrl { get; set; }
    public int IdAuthor { get; set; }
    public string AuthorName { get; set; }
    public TimeSpan TimeCosts { get; set; }
    public int NumberOfPersons { get; set; }

    public User Author { get; set; }

    public List<Step> StepOfCooking { get; set; } = new List<Step>();
    public List<Ingridient> IngridientForCooking { get; set; } = new List<Ingridient>();
    public List<Tag> Tags { get; set; } = new List<Tag>();
    public List<User> UsersLikes { get; set; } = new List<User>();
    public List<User> UsersStars { get; set; } = new List<User>();

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
