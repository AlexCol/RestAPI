using System.Security.Claims;

namespace RestAPI.Services.Crypto;

public interface ITokenService
{
    string GenerateAccesToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);


}
