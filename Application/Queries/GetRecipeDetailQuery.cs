using Application.Models.Result;
using MediatR;

namespace Application.Queries;

public class GetRecipeDetailQuery : IRequest<RecipeDetailResult>
{
    public int Id { get; set; }

    public GetRecipeDetailQuery( int id )
    {
        Id = id;
    }
}
