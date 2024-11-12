using Application.Querys;
using Domain.Entities.RecipeEntities;
using Domain.Interfaces.RecipeInterfaces;
using MediatR;

namespace Application.Querys.Handlers;
public class GetAllTagQueryHandler : IRequestHandler<GetAllTagQuery, IReadOnlyList<Tag>>
{
    private readonly ITagRepository _tagRepository;

    public GetAllTagQueryHandler( ITagRepository tagRepository )
    {
        _tagRepository = tagRepository;
    }

    public async Task<IReadOnlyList<Tag>> Handle( GetAllTagQuery request, CancellationToken cancellationToken )
    {
        var tags = await _tagRepository.GetAllAsync( cancellationToken );
        return tags;
    }
}
