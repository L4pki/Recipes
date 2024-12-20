using Application.Models.Result;
using Domain.Interfaces.RecipeInterfaces;
using MediatR;

namespace Application.Queries.Handlers;

public class GetRecipeByAuthorQueryHandler : IRequestHandler<GetRecipeByAuthorQuery, RecipeListResult>
{
    private readonly IRecipeRepository _recipeRepository;

    public GetRecipeByAuthorQueryHandler( IRecipeRepository recipeRepository )
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<RecipeListResult> Handle( GetRecipeByAuthorQuery request, CancellationToken cancellationToken )
    {
        var result = await _recipeRepository.GetByAuthorAsync( request.IdAuthor, cancellationToken );
        return new RecipeListResult( result, "Рецепт успешно получен!" );
    }
}
