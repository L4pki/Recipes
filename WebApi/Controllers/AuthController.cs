using Application.Commands.UserCommands;
using Application.Models.Result;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dto.UserDto;
using WebAPI.Services;

namespace WebAPI.Controllers;

[ApiController]
[Route( "auth" )]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMediator _mediator;

    public AuthController(
        IAuthService authService,
        IMediator mediatR )
    {
        _authService = authService;
        _mediator = mediatR;
    }

    [HttpPost( "register" )]
    [ProducesResponseType( typeof( string[] ), StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( UserResult ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Register( [FromBody] CreateUserDto user )
    {
        var newRefreshToken = _authService.GenerateRefreshToken();
        UserResult serviceAnswer = await _mediator.Send( new RegisterUserCommand( user.Login, user.Password, user.Name, user.About, newRefreshToken ) );

        if ( serviceAnswer.Message == "Регистрация прошла успешно!" )
        {
            string token = _authService.GenerateJwtToken( serviceAnswer.User );
            return Ok( new { Token = token, serviceAnswer.Message } );
        }
        return BadRequest( serviceAnswer );
    }

    [HttpPost( "login" )]
    [ProducesResponseType( typeof( string[] ), StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( UserResult ), StatusCodes.Status400BadRequest )]
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
    [ProducesResponseType( typeof( string ), StatusCodes.Status200OK )]
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
            var newRefreshToken = _authService.GenerateRefreshToken();
            string answer = await _mediator.Send( new UpdateTokenCommand( userClaims.Id, newRefreshToken ) );
            return Ok( new
            {
                Answer = answer,
                Token = newJwtToken
            } );
        }
        return Unauthorized();
    }
}
