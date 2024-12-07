using Application.Models.Result;
using Domain.Interfaces.RecipeInterfaces;
using MediatR;

namespace Application.Queries.Handlers;
public class GetMostLikedRecipeQueryHandler : IRequestHandler<GetMostLikedRecipeQuery, RecipeListResult>
{
    private readonly IRecipeRepository _recipeRepository;

    public GetMostLikedRecipeQueryHandler( IRecipeRepository recipeRepository )
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<RecipeListResult> Handle( GetMostLikedRecipeQuery request, CancellationToken cancellationToken )
    {
        var result = await _recipeRepository.GetMostLikedRecipesAsync( cancellationToken );
        return new RecipeListResult( result, "Рецепт успешно получен!" );
    }
}
