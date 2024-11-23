using Application.Commands.UserCommands;
using Application.Models.Result;
using Application.Queries;
using Azure.Core;
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
        var newRefreshToken = _authService.GenerateRefreshToken();
        UserResult serviceAnswer = await _mediator.Send( new RegisterUserCommand( user.Login, user.Password, user.Name, user.About, newRefreshToken ) );

        if ( serviceAnswer.Message == "Регистрация прошла успешно!" )
        {
            string token = _authService.GenerateJwtToken( serviceAnswer.User );
            return Ok( new { Token = token, RefreshToken = newRefreshToken, serviceAnswer.Message } );
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
    [HttpGet( "token" )]
    public async Task<IActionResult> RefreshToken()
    {
        string token = HttpContext.Request.Headers[ "Authorization" ].ToString().Replace( "Bearer ", "" );
        UserClaimsDto userClaims = _authService.GetUserClaims( token );

        var user = await _mediator.Send( new GetUserByLoginQuery( userClaims.Login ) );
        if ( user == null )
        {
            return Unauthorized();
        }
        if ( user.RefreshToken != "" )
        {
            var newJwtToken = _authService.GenerateJwtToken( user );
            return Ok( new
            {
                Token = newJwtToken
            } );
        }
        return Unauthorized();
    }

    [Authorize]
    [HttpPost( "update" )]
    public async Task<IActionResult> UpdateUserAsync( [FromBody] UpdateUserDto user )
    {
        string token = HttpContext.Request.Headers[ "Authorization" ].ToString().Replace( "Bearer ", "" );

        try
        {
            UserClaimsDto userClaims = _authService.GetUserClaims( token );

            UserResult serviceAnswer = await _mediator.Send( new UpdateUserCommand( user.Password, user.Name, user.About, userClaims.Login ) );

            return Ok( new { serviceAnswer.Message } );
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
    [HttpPost( "recipe/star/{id}" )]
    public async Task<IActionResult> StarUserAsync( int id )
    {
        string token = HttpContext.Request.Headers[ "Authorization" ].ToString().Replace( "Bearer ", "" );

        try
        {
            UserClaimsDto userClaims = _authService.GetUserClaims( token );
            return Ok( await _mediator.Send( new StarUserCommand( id, userClaims.Id ) ) );
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
    [HttpGet( "recipe/star" )]
    public async Task<IActionResult> GetStarRecipeAsync()
    {
        string token = HttpContext.Request.Headers[ "Authorization" ].ToString().Replace( "Bearer ", "" );

        try
        {
            UserClaimsDto userClaims = _authService.GetUserClaims( token );
            return Ok( await _mediator.Send( new GetStarRecipeQuery( userClaims.Id ) ) );
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
    [HttpGet( "recipe/like" )]
    public async Task<IActionResult> GetLikedRecipesAsync()
    {
        string token = HttpContext.Request.Headers[ "Authorization" ].ToString().Replace( "Bearer ", "" );

        try
        {
            UserClaimsDto userClaims = _authService.GetUserClaims( token );
            return Ok( await _mediator.Send( new GetLikedRecipeQuery( userClaims.Id ) ) );
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
    [HttpGet( "info" )]
    public async Task<IActionResult> InfoUserAsync()
    {
        string token = HttpContext.Request.Headers[ "Authorization" ].ToString().Replace( "Bearer ", "" );

        try
        {
            UserClaimsDto userClaims = _authService.GetUserClaims( token );
            return Ok( await _mediator.Send( new GetUserInfoByIdQuery( userClaims.Id ) ) );
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
    [HttpGet( "recipes" )]
    public async Task<IActionResult> PersonalRecipeAsync()
    {
        string token = HttpContext.Request.Headers[ "Authorization" ].ToString().Replace( "Bearer ", "" );

        try
        {
            UserClaimsDto userClaims = _authService.GetUserClaims( token );
            return Ok( await _mediator.Send( new GetRecipeByAuthorQuery( userClaims.Id ) ) );
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
}
