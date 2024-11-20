using Application.Models.Result;
using Domain.Interfaces.RecipeInterfaces;
using MediatR;

namespace Application.Queries.Handlers;

public class GetRecipeByTagOrNameQueryHandler : IRequestHandler<GetRecipeByTagOrNameQuery, RecipeListResult>
{
    private readonly IRecipeRepository _recipeRepository;

    public GetRecipeByTagOrNameQueryHandler( IRecipeRepository recipeRepository )
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<RecipeListResult> Handle( GetRecipeByTagOrNameQuery request, CancellationToken cancellationToken )
    {
        var result = await _recipeRepository.GetByTagsOrNameAsync( request.SearchString, cancellationToken );
        return new RecipeListResult( result, "Рецепт успешно получен!" );
    }
}
