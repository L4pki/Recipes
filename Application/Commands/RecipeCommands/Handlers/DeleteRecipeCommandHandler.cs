using Domain.Interfaces.RecipeInterfaces;
using MediatR;

namespace Application.Commands.RecipeCommands.Handlers;

public class DeleteRecipeCommandHandler : IRequestHandler<DeleteRecipeCommand, string>
{
    private readonly IRecipeRepository _recipeRepository;

    public DeleteRecipeCommandHandler( IRecipeRepository recipeRepository )
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<string> Handle( DeleteRecipeCommand request, CancellationToken cancellationToken )
    {
        return await _recipeRepository.DeleteAsync( request.Id, cancellationToken );
    }
}
