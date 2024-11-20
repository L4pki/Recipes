using Application.Commands.RecipeCommands;
using Application.Models.Result;
using Application.Querys;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Dto.RecipeDto;
using WebAPI.Dto.UserDto;
using WebAPI.Services;

namespace WebAPI.Controllers;

[ApiController]
[Route( "recipe" )]

public class RecipeController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMediator _mediator;
    public RecipeController( IMediator mediator, IAuthService authService )
    {
        _mediator = mediator;
        _authService = authService;
    }

    [Authorize]
    [HttpPost( "create" )]
    public async Task<IActionResult> CreateRecipeAsync( [FromBody] CreateRecipeDto recipe )
    {
        string token = HttpContext.Request.Headers[ "Authorization" ].ToString().Replace( "Bearer ", "" );

        try
        {
            UserClaimsDto userClaims = _authService.GetUserClaims( token );
            RecipeResult newRecipe = await _mediator.Send(
                    new CreateRecipeCommand(
                        recipe.Name,
                        recipe.ShortDescription,
                        recipe.PhotoUrl,
                        userClaims.Id,
                        recipe.TimeCosts,
                        recipe.NumberOfPersons,
                        recipe.Ingridients.ToArray(),
                        recipe.Steps.ToArray(),
                        recipe.Tags.ToArray() ) );

            return Ok( newRecipe );
        }
        catch ( SecurityTokenException )
        {
            return BadRequest( "Недействительный токен." );
        }
        catch
        {
            return BadRequest( "Неизвестная ошибка" );
        }
    }

    [Authorize]
    [HttpPost( "update" )]
    public async Task<IActionResult> UpdateRecipeAsync( [FromBody] UpdateRecipeDto recipe )
    {

        string token = HttpContext.Request.Headers[ "Authorization" ].ToString().Replace( "Bearer ", "" );

        try
        {
            UserClaimsDto userClaims = _authService.GetUserClaims( token );
            RecipeResult recipeInDb = await _mediator.Send( new GetRecipeByIdQuery( recipe.IdRecipe ) );
            if ( recipeInDb.Recipe != null )
            {
                if ( recipeInDb.Recipe.IdAuthor == userClaims.Id )
                {
                    RecipeResult newRecipe = await _mediator.Send( new UpdateRecipeCommand(
                        recipe.IdRecipe,
                        recipe.Name,
                        recipe.ShortDescription,
                        recipe.PhotoUrl,
                        userClaims.Id,
                        recipe.TimeCosts,
                        recipe.NumberOfPersons,
                        recipe.Ingridients.ToArray(),
                        recipe.Steps.ToArray(),
                        recipe.Tags.ToArray() ) );

                    return Ok( newRecipe );
                }
                return BadRequest( "Не является вашим рецептом!" );
            }
            return BadRequest( "Рецепт не найден!" );
        }
        catch ( SecurityTokenException )
        {
            return BadRequest( "Недействительный токен." );
        }
        catch
        {
            return BadRequest( "Неизвестная ошибка" );
        }
    }

    [Authorize]
    [HttpDelete( "delete" )]
    public async Task<IActionResult> DeleteRecipeAsync( [FromBody] int recipeId )
    {
        string token = HttpContext.Request.Headers[ "Authorization" ].ToString().Replace( "Bearer ", "" );

        try
        {
            UserClaimsDto userClaims = _authService.GetUserClaims( token );
            RecipeResult recipe = await _mediator.Send( new GetRecipeByIdQuery( recipeId ) );
            if ( recipe.Recipe != null )
            {
                if ( recipe.Recipe.IdAuthor == userClaims.Id )
                {
                    return Ok( await _mediator.Send( new DeleteRecipeCommand( recipeId, userClaims.Id ) ) );
                }
                return BadRequest( "Не является вашим рецептом!" );
            }
            return BadRequest( "Рецепт не найден!" );
        }
        catch ( SecurityTokenException )
        {
            return BadRequest( "Недействительный токен." );
        }
        catch
        {
            return BadRequest( "Неизвестная ошибка" );
        }
    }

    [Authorize]
    [HttpPost( "recipe/like" )]
    public async Task<IActionResult> LikeRecipeAsync( [FromBody] int recipeId )
    {
        string token = HttpContext.Request.Headers[ "Authorization" ].ToString().Replace( "Bearer ", "" );

        try
        {
            UserClaimsDto userClaims = _authService.GetUserClaims( token );
            return Ok( await _mediator.Send( new LikeRecipeCommand( recipeId, userClaims.Id ) ) );
        }
        catch ( SecurityTokenException )
        {
            return BadRequest( "Недействительный токен." );
        }
        catch
        {
            return BadRequest( "Неизвестная ошибка" );
        }
    }

    [HttpPost( "search" )]
    public async Task<IActionResult> RecipeListBySearchString( [FromBody] string searchString )
    {
        return Ok( await _mediator.Send( new GetRecipeByTagOrNameQuery( searchString ) ) );
    }

    [HttpPost( "tag/create" )]
    public async Task<IActionResult> CreateTagAsync( [FromBody] CreateTagDto tag )
    {
        try
        {
            return Ok( await _mediator.Send( new CreateNewTagCommand( tag.Name ) ) );
        }
        catch ( InvalidOperationException ex )
        {
            return BadRequest( ex.Message );
        }
    }

    [HttpGet( "tag/getall" )]
    public async Task<IActionResult> GetAll()
    {
        return Ok( await _mediator.Send( new GetAllTagQuery() ) );
    }

}
