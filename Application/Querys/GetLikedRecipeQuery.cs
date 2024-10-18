using Application.Models.Result;
using MediatR;

namespace Application.Querys;
public class GetLikedRecipeQuery : IRequest<RecipeListResult>
{
    public int Id { get; set; }

    public GetLikedRecipeQuery( int id )
    {
        Id = id;
    }
}
