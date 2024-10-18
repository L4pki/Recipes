using Application.Models.Result;
using MediatR;

namespace Application.Querys;
public class GetRecipeByAuthorQuery : IRequest<RecipeListResult>
{
    public int IdAuthor { get; set; }

    public GetRecipeByAuthorQuery( int idAuthor )
    {
        IdAuthor = idAuthor;
    }
}
