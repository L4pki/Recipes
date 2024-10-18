using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Dto.UserDto;

namespace WebAPI.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;

    public AuthService( IConfiguration configuration )
    {
        _configuration = configuration;
    }

    public string GenerateJwtToken( User user )
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Aud, _configuration["JwtSettings:Audience"])
        };

        var key = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( _configuration[ "JwtSettings:SecretKey" ] ) );
        var creds = new SigningCredentials( key, SecurityAlgorithms.HmacSha256 );

        var token = new JwtSecurityToken(
            issuer: _configuration[ "JwtSettings:Issuer" ],
            audience: _configuration[ "JwtSettings:Audience" ],
            claims: claims,
            expires: DateTime.Now.AddMinutes( 30 ),
            signingCredentials: creds );

        return new JwtSecurityTokenHandler().WriteToken( token );
    }

    public UserClaimsDto GetUserClaims( string token )
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken( token ) as JwtSecurityToken;

        if ( jwtToken == null )
        {
            throw new SecurityTokenException( "Invalid token." );
        }

        return new UserClaimsDto
        {
            Id = int.Parse( jwtToken.Claims.First( claim => claim.Type == JwtRegisteredClaimNames.Sub ).Value ),
            Login = jwtToken.Claims.First( claim => claim.Type == ClaimTypes.Name ).Value
        };
    }
}
