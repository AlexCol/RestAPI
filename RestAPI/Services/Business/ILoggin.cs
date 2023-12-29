using RestAPI.Data.VO;

namespace RestAPI.Services.Business;

public interface ILoggin
{
    TokenVO ValidadeCredentials(UserVO user);
    TokenVO ValidadeCredentials(TokenVO token);
    bool RevokeToken(string userName);
}
