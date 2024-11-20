using Domain.Entities;
using WebAPI.Dto.UserDto;

namespace WebAPI.Services;

public interface IAuthService
{
    public string GenerateJwtToken( User user );

    public UserClaimsDto GetUserClaims( string token );
}
