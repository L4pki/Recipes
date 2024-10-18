using Application.Commands.UserCommands;
using Application.Models.Result;
using Application.Querys;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Dto.UserDto;
using WebAPI.Services;

namespace WebAPI.Controllers;

[ApiController]
[Route( "user" )]
public class UserController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMediator _mediator;

    public UserController(
        IAuthService authService,
        IMediator mediatR )
    {
        _authService = authService;
        _mediator = mediatR;
    }

    [HttpPost( "register" )]
    public async Task<IActionResult> Register( [FromBody] CreateUserDto user )
    {
        UserResult serviceAnswer = await _mediator.Send( new RegisterUserCommand( user.Login, user.Password, user.Name, user.About ) );

        if ( serviceAnswer.Message == "Регистрация прошла успешно!" )
        {
            string token = _authService.GenerateJwtToken( serviceAnswer.User );
            return Ok( new { Token = token, serviceAnswer.Message } );
        }
        return BadRequest( serviceAnswer );
    }

    [HttpPost( "login" )]
    public async Task<IActionResult> Login( [FromBody] LoginUserDto user )
    {
        UserResult serviceAnswer = await _mediator.Send( new LoginUserCommand( user.Login, user.Password ) );
        if ( serviceAnswer.Message == "Вход прошел успешно!" )
        {
            string token = _authService.GenerateJwtToken( serviceAnswer.User );
            return Ok( new { Token = token, serviceAnswer.Message } );
        }
        return BadRequest( serviceAnswer );
    }

    [Authorize]
    [HttpPost( "update" )]
    public async Task<IActionResult> UpdateUserAsync( [FromBody] UpdateUserDto user )
    {
        string token = HttpContext.Request.Headers[ "Authorization" ].ToString().Replace( "Bearer ", "" );

        try
        {
            var userClaims = _authService.GetUserClaims( token );

            UserResult serviceAnswer = await _mediator.Send( new UpdateUserCommand( user.Login, user.Password, user.Name, user.About, userClaims.Login ) );

            string newToken = _authService.GenerateJwtToken( serviceAnswer.User );
            return Ok( new { Token = newToken, serviceAnswer.Message } );
        }
        catch ( SecurityTokenException )
        {
            return Unauthorized( "Недействительный токен." );
        }
        catch
        {
            return Unauthorized( "Неизвестная ошибка" );
        }
    }

    [Authorize]
    [HttpPost( "recipe/star" )]
    public async Task<IActionResult> StarUserAsync( [FromBody] int recipeId )
    {
        string token = HttpContext.Request.Headers[ "Authorization" ].ToString().Replace( "Bearer ", "" );

        try
        {
            var userClaims = _authService.GetUserClaims( token );
            return Ok( await _mediator.Send( new StarUserCommand( recipeId, userClaims.Id ) ) );
        }
        catch ( SecurityTokenException )
        {
            return Unauthorized( "Недействительный токен." );
        }
        catch
        {
            return Unauthorized( "Неизвестная ошибка" );
        }
    }

    [Authorize]
    [HttpGet( "recipe/star" )]
    public async Task<IActionResult> GetStarRecipeAsync()
    {
        string token = HttpContext.Request.Headers[ "Authorization" ].ToString().Replace( "Bearer ", "" );

        try
        {
            var userClaims = _authService.GetUserClaims( token );
            return Ok( await _mediator.Send( new GetStarRecipeQuery( userClaims.Id ) ) );
        }
        catch ( SecurityTokenException )
        {
            return Unauthorized( "Недействительный токен." );
        }
        catch
        {
            return Unauthorized( "Неизвестная ошибка" );
        }
    }

    [Authorize]
    [HttpGet( "recipe/like" )]
    public async Task<IActionResult> GetLikedRecipesAsync()
    {
        string token = HttpContext.Request.Headers[ "Authorization" ].ToString().Replace( "Bearer ", "" );

        try
        {
            var userClaims = _authService.GetUserClaims( token );
            return Ok( await _mediator.Send( new GetLikedRecipeQuery( userClaims.Id ) ) );
        }
        catch ( SecurityTokenException )
        {
            return Unauthorized( "Недействительный токен." );
        }
        catch
        {
            return Unauthorized( "Неизвестная ошибка" );
        }
    }

    [Authorize]
    [HttpGet( "info" )]
    public async Task<IActionResult> InfoUserAsync()
    {
        string token = HttpContext.Request.Headers[ "Authorization" ].ToString().Replace( "Bearer ", "" );

        try
        {
            var userClaims = _authService.GetUserClaims( token );
            return Ok( await _mediator.Send( new GetUserByLoginQuery( userClaims.Login ) ) );
        }
        catch ( SecurityTokenException )
        {
            return Unauthorized( "Недействительный токен." );
        }
        catch
        {
            return Unauthorized( "Неизвестная ошибка" );
        }
    }

    [Authorize]
    [HttpGet( "recipes" )]
    public async Task<IActionResult> PersonalRecipeAsync()
    {
        string token = HttpContext.Request.Headers[ "Authorization" ].ToString().Replace( "Bearer ", "" );

        try
        {
            var userClaims = _authService.GetUserClaims( token );
            return Ok( await _mediator.Send( new GetRecipeByAuthorQuery( userClaims.Id ) ) );
        }
        catch ( SecurityTokenException )
        {
            return Unauthorized( "Недействительный токен." );
        }
        catch
        {
            return Unauthorized( "Неизвестная ошибка" );
        }
    }
}
