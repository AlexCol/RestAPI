using RestAPI.Data.VO;

namespace RestAPI.Services.Repository;

public interface IUserRepository
{
    User ValidadeCredentials(UserVO user);
}

