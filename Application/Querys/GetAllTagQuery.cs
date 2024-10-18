using Domain.Entities.RecipeEntities;
using MediatR;

namespace Application.Querys;
public class GetAllTagQuery : IRequest<IReadOnlyList<Tag>>
{
    public GetAllTagQuery()
    {
    }
}
