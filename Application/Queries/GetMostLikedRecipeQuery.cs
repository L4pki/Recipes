using Application.Models.Result;
using MediatR;

namespace Application.Queries;

public class GetMostLikedRecipeQuery : IRequest<RecipeListResult>
{
    public GetMostLikedRecipeQuery()
    {
    }
}
