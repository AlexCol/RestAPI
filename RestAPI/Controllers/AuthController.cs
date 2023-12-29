using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Data.VO;
using RestAPI.Services.Business;

namespace RestAPI.Controllers;

[ApiVersion("1")]
[ApiController]
[AllowAnonymous] //permite rodar ser autorização
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private ILoggin _login;

    public AuthController(ILoggin login)
    {
        _login = login;
    }

    [HttpPost()]
    [Route("signin")]
    public IActionResult Signin([FromBody] UserVO user)
    {
        if (user == null || user.UserName == null || user.Password == null)
            return BadRequest("Solicitação invalida.");

        var token = _login.ValidadeCredentials(user);
        if (token == null)
            return BadRequest("Senha ou usuário invalidos.");

        return Ok(token);
    }
}
