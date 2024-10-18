using Application.Models.Result;
using Application.Querys;
using Domain.Interfaces.RecipeInterfaces;
using MediatR;

namespace Application.Handlers.RecipeHandlers.Querys;
public class GetLikedRecipesQueryHandler : IRequestHandler<GetLikedRecipeQuery, RecipeListResult>
{
    private readonly IRecipeRepository _recipeRepository;

    public GetLikedRecipesQueryHandler( IRecipeRepository recipeRepository )
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<RecipeListResult> Handle( GetLikedRecipeQuery request, CancellationToken cancellationToken )
    {
        return new RecipeListResult( await _recipeRepository.GetLikedRecipesAsync( request.Id, cancellationToken ), "Рецепт успешно получен!" );
    }
}
