namespace Domain.Entities;

public class Recipes
{
    public int Id { get; private init; }
    public string Name { get; private set; }
    public string ShortDescription { get; set; }
    public string PhotoURL { get; set; }
    public int IdAuthor { get; private set; }
    public string[] Tags { get; private set; }
    public string TimeCosts { get; private set; }
    public int NumberOfPersons { get; private set; }
    public string[] StepOfCooking { get; private set; }
    public string[][] Ingridients { get; private set; }

    public Recipes(
        string name,
        string shortDescription,
        string photoUrl,
        int idAuthor,
        string[] tags,
        string timeCosts,
        int numberOfPersons,
        string[] stepOfCooking,
        string[][] ingridients )
    {
        Name = name;
        ShortDescription = shortDescription;
        PhotoURL = photoUrl;
        IdAuthor = idAuthor;
        Tags = tags;
        Ingridients = ingridients;
        TimeCosts = timeCosts;
        NumberOfPersons = numberOfPersons;
        StepOfCooking = stepOfCooking;
    }


}
