using Application.Models.Result;
using Domain.Interfaces.RecipeInterfaces;
using MediatR;

namespace Application.Queries.Handlers;

public class CheckLikeStarRecipeByUserQueryHandler : IRequestHandler<CheckLikeStarRecipeByUserQuery, RecipeStatusResult>
{
    private readonly IRecipeRepository _recipeRepository;

    public CheckLikeStarRecipeByUserQueryHandler( IRecipeRepository recipeRepository )
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<RecipeStatusResult> Handle( CheckLikeStarRecipeByUserQuery request, CancellationToken cancellationToken )
    {
        var result = await _recipeRepository.RecipeLikeStarStatus( request.IdUser, request.IdRecipe, cancellationToken );
        return new RecipeStatusResult( result[ 0 ], result[ 1 ] );
    }
}
