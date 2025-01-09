using Application.Commands.RecipeCommands;
using Application.Models.Result;
using Application.Queries;
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
    public async Task<IActionResult> CreateRecipeAsync( [FromForm] CreateRecipeRequest request )
    {
        var recipe = request.Recipe;
        var image = request.Image;

        string token = HttpContext.Request.Headers[ "Authorization" ].ToString().Replace( "Bearer ", "" );

        try
        {
            UserClaimsDto userClaims = _authService.GetUserClaims( token );

            string imageUrl = await SaveImageAsync( image );

            RecipeResult newRecipe = await _mediator.Send(
                    new CreateRecipeCommand(
                        recipe.Name,
                        recipe.ShortDescription,
                        imageUrl,
                        userClaims.Id,
                        userClaims.Login,
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
        /* catch
         {
             return BadRequest( "Неизвестная ошибка" );
         }*/
    }

    private async Task<string> SaveImageAsync( IFormFile image )
    {
        if ( image == null || image.Length == 0 )
            throw new ArgumentException( "Изображение не может быть пустым." );

        var fileName = Path.GetFileName( image.FileName );
        var filePath = Path.Combine( Directory.GetCurrentDirectory(), "wwwroot/images/recipes", fileName );
        Directory.CreateDirectory( Path.GetDirectoryName( filePath ) );

        using ( var stream = new FileStream( filePath, FileMode.Create ) )
        {
            await image.CopyToAsync( stream );
        }
        return $"https://localhost:7217/images/recipes/{fileName}";
    }

    [Authorize]
    [HttpPost( "update/{id}" )]
    public async Task<IActionResult> UpdateRecipeAsync( [FromForm] UpdateRecipeRequest request, int id )
    {
        string token = HttpContext.Request.Headers[ "Authorization" ].ToString().Replace( "Bearer ", "" );

        try
        {
            UserClaimsDto userClaims = _authService.GetUserClaims( token );
            RecipeResult recipeInDb = await _mediator.Send( new GetRecipeByIdQuery( id ) );

            if ( recipeInDb.Recipe != null )
            {
                if ( recipeInDb.Recipe.IdAuthor == userClaims.Id )
                {
                    string imageUrl = recipeInDb.Recipe.PhotoUrl;
                    var recipe = request.Recipe;

                    if ( request.Image != null )
                    {
                        imageUrl = await SaveImageAsync( request.Image );
                    }

                    RecipeResult newRecipe = await _mediator.Send( new UpdateRecipeCommand(
                        id,
                        recipe.Name,
                        recipe.ShortDescription,
                        imageUrl,
                        userClaims.Id,
                        recipe.TimeCosts,
                        recipe.NumberOfPersons,
                        recipe.Ingridients.ToArray(),
                        recipe.Steps.ToArray(),
                        recipe.Tags.ToArray()
                    ) );

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
            return BadRequest( "Ошибка неизвестна" );
        }
    }

    [Authorize]
    [HttpDelete( "delete/{id}" )]
    public async Task<IActionResult> DeleteRecipeAsync( int id )
    {
        string token = HttpContext.Request.Headers[ "Authorization" ].ToString().Replace( "Bearer ", "" );

        try
        {
            UserClaimsDto userClaims = _authService.GetUserClaims( token );
            RecipeResult recipe = await _mediator.Send( new GetRecipeByIdQuery( id ) );
            if ( recipe.Recipe != null )
            {
                if ( recipe.Recipe.IdAuthor == userClaims.Id )
                {
                    return Ok( await _mediator.Send( new DeleteRecipeCommand( id, userClaims.Id ) ) );
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
    [HttpPost( "like/{id}" )]
    public async Task<IActionResult> LikeRecipeAsync( int id )
    {
        string token = HttpContext.Request.Headers[ "Authorization" ].ToString().Replace( "Bearer ", "" );

        try
        {
            UserClaimsDto userClaims = _authService.GetUserClaims( token );
            return Ok( await _mediator.Send( new LikeRecipeCommand( id, userClaims.Id ) ) );
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

    [HttpGet( "search/{searchString}" )]
    public async Task<IActionResult> RecipeListBySearchString( string searchString )
    {
        return Ok( await _mediator.Send( new GetRecipeByTagOrNameQuery( searchString ) ) );
    }

    [HttpGet( "mostliked" )]
    public async Task<IActionResult> RecipesMostLikedAsync()
    {
        return Ok( await _mediator.Send( new GetMostLikedRecipeQuery() ) );
    }

    [Authorize]
    [HttpGet( "status/{id}" )]
    public async Task<IActionResult> RecipeStatusAsync( int id )
    {
        string token = HttpContext.Request.Headers[ "Authorization" ].ToString().Replace( "Bearer ", "" );

        try
        {
            UserClaimsDto userClaims = _authService.GetUserClaims( token );
            RecipeStatusResult result = await _mediator.Send( new CheckLikeStarRecipeByUserQuery( userClaims.Id, id ) );
            return Ok( result );
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

    [HttpGet( "detail/{id}" )]
    public async Task<IActionResult> RecipeDetailAsync( int id )
    {
        try
        {
            RecipeDetailResult result = await _mediator.Send( new GetRecipeDetailQuery( id ) );
            return Ok( result );
        }
        catch
        {
            return BadRequest( "Неизвестная ошибка" );
        }
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
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok( await _mediator.Send( new GetAllTagQuery() ) );
    }

    [HttpGet( "tag/getpopular" )]
    public async Task<IActionResult> GetPopularAsync()
    {
        return Ok( await _mediator.Send( new GetPopularTagQuery() ) );
    }
}
