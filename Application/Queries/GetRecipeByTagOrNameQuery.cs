using Application.Models.Result;
using MediatR;

namespace Application.Queries;

public class GetRecipeByTagOrNameQuery : IRequest<RecipeListResult>
{
    public string SearchString { get; set; }

    public GetRecipeByTagOrNameQuery( string searchString )
    {
        SearchString = searchString;
    }
}
