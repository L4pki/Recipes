using Application.Models.Result;
using Domain.Interfaces.RecipeInterfaces;
using MediatR;

namespace Application.Queries.Handlers;
public class GetPopularTagQueryHandler : IRequestHandler<GetPopularTagQuery, TagListResult>
{
    private readonly ITagRepository _tagRepository;

    public GetPopularTagQueryHandler( ITagRepository tagRepository )
    {
        _tagRepository = tagRepository;
    }

    public async Task<TagListResult> Handle( GetPopularTagQuery request, CancellationToken cancellationToken )
    {
        var result = await _tagRepository.GetPopularTagAsync( cancellationToken );
        return new TagListResult( result );
    }
}

