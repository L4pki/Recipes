using Application.Models.Result;
using Application.Querys;
using Domain.Interfaces.RecipeInterfaces;
using MediatR;

namespace Application.Handlers.RecipeHandlers.Querys;
public class GetRecipeByIdQueryHandler : IRequestHandler<GetRecipeByIdQuery, RecipeResult>
{
    private readonly IRecipeRepository _recipeRepository;

    public GetRecipeByIdQueryHandler( IRecipeRepository recipeRepository )
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<RecipeResult> Handle( GetRecipeByIdQuery request, CancellationToken cancellationToken )
    {
        var result = await _recipeRepository.GetByIdAsync( request.Id, cancellationToken );
        return new RecipeResult( result, "Рецепт успешно получен!" );
    }
}
