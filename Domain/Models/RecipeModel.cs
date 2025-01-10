using Domain.Entities.RecipeEntities;

namespace Domain.Models;

public class RecipeModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string PhotoUrl { get; set; }
    public int IdAuthor { get; set; }
    public int TimeCosts { get; set; }
    public List<Tag> Tags { get; set; }
    public List<Step> StepOfCooking { get; set; }
    public List<Ingridient> IngridientForCooking { get; set; }
    public string AuthorName { get; set; }
    public int NumberOfPersons { get; set; }
    public int UsersLikesCount { get; set; }
    public int UsersStarsCount { get; set; }
}
