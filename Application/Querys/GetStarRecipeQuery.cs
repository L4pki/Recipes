using Application.Models.Result;
using MediatR;

namespace Application.Querys;
public class GetStarRecipeQuery : IRequest<RecipeListResult>
{
    public int Id { get; set; }

    public GetStarRecipeQuery( int id )
    {
        Id = id;
    }
}
