using Domain.Entities.RecipeEntities;
using MediatR;

namespace Application.Queries;

public class GetAllTagQuery : IRequest<IReadOnlyList<Tag>>
{
    public GetAllTagQuery()
    {
    }
}
