using RestAPI.Data.VO;
using RestAPI.Model;

namespace RestAPI.Services.Repository;

public interface IUserRepository
{
    User ValidadeCredentials(UserVO user);

    User RefreshUserInfo(User user);

    List<Claims> ListUserClaims(User user);
}

