using Application.Models.Result;
using MediatR;

namespace Application.Queries;

public class CheckLikeStarRecipeByUserQuery : IRequest<RecipeStatusResult>
{
    public int IdUser { get; set; }
    public int IdRecipe { get; set; }

    public CheckLikeStarRecipeByUserQuery( int idUser, int idRecipe )
    {
        IdUser = idUser;
        IdRecipe = idRecipe;
    }
}
