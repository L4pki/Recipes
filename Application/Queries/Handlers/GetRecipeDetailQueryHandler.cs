using Application.Models.Result;
using Domain.Interfaces.RecipeInterfaces;
using MediatR;

namespace Application.Queries.Handlers;
public class GetRecipeDetailQueryHandler : IRequestHandler<GetRecipeDetailQuery, RecipeDetailResult>
{
    private readonly IRecipeRepository _recipeRepository;

    public GetRecipeDetailQueryHandler( IRecipeRepository recipeRepository )
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<RecipeDetailResult> Handle( GetRecipeDetailQuery request, CancellationToken cancellationToken )
    {
        var result = await _recipeRepository.GetDetailByIdAsync( request.Id, cancellationToken );
        return new RecipeDetailResult( result, "Рецепт успешно получен!" );
    }
}
