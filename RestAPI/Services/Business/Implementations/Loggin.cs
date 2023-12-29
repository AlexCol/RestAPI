using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using RestAPI.Configurations;
using RestAPI.Data.VO;
using RestAPI.Model;
using RestAPI.Services.Crypto;
using RestAPI.Services.Repository;
using Serilog;

namespace RestAPI.Services.Business.Implementations;

public class Loggin : ILoggin
{
    private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
    private TokenConfiguration _tokenConfiguration;
    private IUserRepository _userRespository;
    private readonly ITokenService _tokenService;

    public Loggin(TokenConfiguration tokenConfiguration, IUserRepository userRespository, ITokenService tokenService)
    {
        _tokenConfiguration = tokenConfiguration;
        _userRespository = userRespository;
        _tokenService = tokenService;
    }

    public TokenVO ValidadeCredentials(UserVO userCredentials)
    {
        var user = _userRespository.ValidadeCredentials(userCredentials);
        if (user == null) return null;

        var claims = new List<Claim>() {
            //o ideial é ter a chave-valor em banco para associar por usuário
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
        };

        // + minhas claims
        List<Claims> minhasClaims = _userRespository.ListUserClaims(user);
        if (minhasClaims != null)
        {
            foreach (Claims minhaClaim in minhasClaims)
            {
                claims.Add(
                    new Claim(minhaClaim.Key, minhaClaim.Value)
                );
            }
        }

        var accessToken = _tokenService.GenerateAccesToken(claims);
        var refreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_tokenConfiguration.DaysToExpire);

        _userRespository.RefreshUserInfo(user);

        DateTime createDate = DateTime.Now;
        DateTime expirationDate = createDate.AddMinutes(_tokenConfiguration.Minutes);

        return new TokenVO(
            true,
            createDate.ToString(DATE_FORMAT),
            expirationDate.ToString(DATE_FORMAT),
            accessToken,
            refreshToken
        );
    }

    public TokenVO ValidadeCredentials(TokenVO token)
    {
        var accessToken = token.AccesToken;
        var refreshToken = token.RefreshToken;
        var pricipal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        var userName = pricipal.Identity.Name;
        Log.Error(userName);

        var user = _userRespository.ValidadeCredentials(userName);

        if (userName == null ||
            user.RefreshToken != refreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.Now
        )
            return null;

        accessToken = _tokenService.GenerateAccesToken(pricipal.Claims);
        refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;

        _userRespository.RefreshUserInfo(user);

        DateTime createDate = DateTime.Now;
        DateTime expirationDate = createDate.AddMinutes(_tokenConfiguration.Minutes);

        return new TokenVO(
            true,
            createDate.ToString(DATE_FORMAT),
            expirationDate.ToString(DATE_FORMAT),
            accessToken,
            refreshToken
        );
    }

    public bool RevokeToken(string userName)
    {
        return _userRespository.RevokeToken(userName);
    }
}
