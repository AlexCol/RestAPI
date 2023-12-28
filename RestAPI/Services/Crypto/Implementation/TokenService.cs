using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RestAPI.Configurations;
using Serilog;

namespace RestAPI.Services.Crypto.Implementation;

public class TokenService : ITokenService
{
    private TokenConfiguration _configuration;

    public TokenService(TokenConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccesToken(IEnumerable<Claim> _claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var options = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            audience: _configuration.Audience,
            claims: _claims,
            signingCredentials: signingCredentials,
            expires: DateTime.Now.AddMinutes(_configuration.Minutes)
        );
        var tokenHandler = new JwtSecurityTokenHandler();
        string tokenString = tokenHandler.WriteToken(options);
        Log.Information($"Token1: {tokenString}");
        return tokenString;


        /* outra forma de fazer
                List<Claim> claimsList = new List<Claim>();
                foreach (Claim item in _claims)
                {
                    claimsList.Add(item);
                }
                ClaimsIdentity subject = new ClaimsIdentity(claimsList);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = _configuration.Issuer,
                    Audience = _configuration.Audience,
                    Subject = subject,
                    SigningCredentials = signingCredentials,
                    Expires = DateTime.Now.AddMinutes(_configuration.Minutes)
                };

                var tokenHandler2 = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString2 = tokenHandler2.WriteToken(token);
                Log.Information($"Token2: {tokenString2}");
                return tokenString2;
        */
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = _configuration.Issuer,
            ValidAudience = _configuration.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret)),
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;

        ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture))
            throw new SecurityTokenException("Invalid Token.");

        return principal;
    }
}
