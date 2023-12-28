using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RestAPI.Data.VO;

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

    public User RefreshUserInfo(User user)
    {
        if (!_context.Users.Any(u => u.Id.Equals(user.Id))) return null;

        var result = _context.Users.Single(u => u.Id.Equals(user.Id));
        _context.Entry(result).CurrentValues.SetValues(user);
        _context.SaveChanges();

        return result;
    }

    private string ComputeHash(string password, SHA256 sHA256)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(password);
        byte[] hashedBytes = sHA256.ComputeHash(inputBytes);

        var builder = new StringBuilder();

        foreach (var item in hashedBytes)
        {
            builder.Append(item.ToString("x2"));
        }
        return builder.ToString();
    }
}
