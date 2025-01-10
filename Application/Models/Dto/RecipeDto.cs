using Domain.Entities.RecipeEntities;
using Domain.Entities;

namespace Application.Models.Dto;

public class RecipeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string PhotoUrl { get; set; }
    public int IdAuthor { get; set; }
    public int TimeCosts { get; set; }
    public int NumberOfPersons { get; set; }
    public List<Step> StepOfCooking { get; set; } = new List<Step>();
    public List<Ingridient> IngridientForCooking { get; set; } = new List<Ingridient>();
    public List<Tag> Tags { get; set; } = new List<Tag>();
    public List<User> UsersLikes { get; set; } = new List<User>();
    public List<User> UsersStars { get; set; } = new List<User>();
}
