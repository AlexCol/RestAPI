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

    [HttpPost]
    [Route("signin")]
    public IActionResult Signin([FromBody] UserVO user)
    {
        if (user == null || user.UserName == null || user.Password == null)
            return BadRequest("Solicitação invalida.");

        var token = _login.ValidadeCredentials(user);
        if (token == null)
            return Unauthorized("Senha ou usuário invalidos.");

        return Ok(token);
    }
    [HttpPost]
    [Route("refresh")]
    public IActionResult Refresh([FromBody] TokenVO tokenVO)
    {
        if (tokenVO == null)
            return BadRequest("Solicitação invalida.");

        var token = _login.ValidadeCredentials(tokenVO);
        if (token == null)
            return BadRequest("Erro ao regerar o token.");

        return Ok(token);
    }

    [HttpGet]
    [Authorize]
    [Route("revoke")]
    public IActionResult Revoke()
    {
        var userName = User.Identity.Name; //devido ao Authorize, ele já consegue buscar o usuário
        var result = _login.RevokeToken(userName);
        if (!result)
            return BadRequest("Erro ao revogar o token");

        return Ok("Token revogado.");
    }
}
