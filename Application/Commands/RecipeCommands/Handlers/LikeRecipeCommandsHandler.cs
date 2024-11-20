using Domain.Interfaces.RecipeInterfaces;
using MediatR;

namespace Application.Commands.RecipeCommands.Handlers;

public class LikeRecipeCommandsHandler : IRequestHandler<LikeRecipeCommand, string>
{
    private readonly IRecipeRepository _recipeRepository;

    public LikeRecipeCommandsHandler( IRecipeRepository recipeRepository )
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<string> Handle( LikeRecipeCommand request, CancellationToken cancellationToken )
    {
        return await _recipeRepository.LikeAsync( request.IdUser, request.IdRecipe, cancellationToken );
    }
}
