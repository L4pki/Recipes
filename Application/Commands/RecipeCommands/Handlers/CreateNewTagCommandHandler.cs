using Application.Models.Result;
using Domain.Entities.RecipeEntities;
using Domain.Interfaces.RecipeInterfaces;
using MediatR;

namespace Application.Commands.RecipeCommands.Handlers;

public class CreateNewTagCommandHandler : IRequestHandler<CreateNewTagCommand, TagResult>
{
    private readonly ITagRepository _tagRepository;

    public CreateNewTagCommandHandler( ITagRepository tagRepository )
    {
        _tagRepository = tagRepository;
    }

    public async Task<TagResult> Handle( CreateNewTagCommand request, CancellationToken cancellationToken )
    {
        if ( string.IsNullOrWhiteSpace( request.Name ) )
        {
            return new TagResult( null, "Ошибка: Не указан тэг" );
        }

        Tag newTag = new Tag()
        {
            Name = request.Name
        };

        await _tagRepository.CreateAsync( newTag, cancellationToken );
        return new TagResult( newTag.Name, "Тэг успешно добавлен!" );
    }
}
