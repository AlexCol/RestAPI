using System.Security.Cryptography;
using System.Text;
using RestAPI.Data.VO;
using RestAPI.Model;
using Serilog;

namespace RestAPI.Services.Repository;

public class UserRepository : IUserRepository
{
    private readonly MySqlContext _context;

    public UserRepository(MySqlContext context)
    {
        _context = context;
    }

    public User ValidadeCredentials(UserVO user)
    {
        var pass = ComputeHash(user.Password, SHA256.Create());
        return _context.Users.FirstOrDefault(u => (u.UserName == user.UserName) && (u.Password == pass));
    }

    public User ValidadeCredentials(string userName)
    {
        return _context.Users.SingleOrDefault(u => u.UserName == userName);
    }

    public User RefreshUserInfo(User user)
    {
        if (!_context.Users.Any(u => u.Id.Equals(user.Id))) return null;

        var result = _context.Users.Single(u => u.Id.Equals(user.Id));
        _context.Entry(result).CurrentValues.SetValues(user);
        _context.SaveChanges();

        return result;
    }

    public bool RevokeToken(string userName)
    {
        var user = _context.Users.SingleOrDefault(u => u.UserName == userName);
        if (user == null) return false;

        user.RefreshToken = null;
        _context.SaveChanges();
        return true;

    }

    public List<Claims> ListUserClaims(User user)
    {
        return _context.Claims.Where(c => c.User.Id == user.Id).ToList();
    }

    private string ComputeHash(string password, HashAlgorithm hashAlgorithm)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(password);
        byte[] hashedBytes = hashAlgorithm.ComputeHash(inputBytes);

        var builder = new StringBuilder();

        foreach (var item in hashedBytes)
        {
            builder.Append(item.ToString("x2"));
        }
        return builder.ToString();
    }
}
