using RestAPI.Data.VO;
using RestAPI.Model;

namespace RestAPI.Services.Repository;

public interface IUserRepository
{
    User ValidadeCredentials(UserVO user);

    User ValidadeCredentials(string userName);

    User RefreshUserInfo(User user);

    bool RevokeToken(string userName);

    List<Claims> ListUserClaims(User user);
}

