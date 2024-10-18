using Application.Models.Result;
using MediatR;

namespace Application.Querys;
public class GetRecipeByIdQuery : IRequest<RecipeResult>
{
    public int Id { get; set; }

    public GetRecipeByIdQuery(int id )
    {
        Id = id;
    }
}
