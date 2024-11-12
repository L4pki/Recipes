using Application.Models.Result;
using Application.Querys;
using Domain.Interfaces.RecipeInterfaces;
using MediatR;

namespace Application.Querys.Handlers;
public class GetStarRecipeQueryHandler : IRequestHandler<GetStarRecipeQuery, RecipeListResult>
{
    private readonly IRecipeRepository _recipeRepository;

    public GetStarRecipeQueryHandler( IRecipeRepository recipeRepository )
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<RecipeListResult> Handle( GetStarRecipeQuery request, CancellationToken cancellationToken )
    {
        return new RecipeListResult( await _recipeRepository.GetFavoriteRecipesAsync( request.Id, cancellationToken ), "Рецепт успешно получен!" );
    }
}
