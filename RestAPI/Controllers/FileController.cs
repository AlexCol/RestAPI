using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using RestAPI.Data.VO;
using RestAPI.Services.Business;
using Serilog;

namespace RestAPI.Controllers;

[ApiVersion("1")]
[Authorize]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class FileController : ControllerBase
{
    private readonly IFileBusiness _fileBusiness;

    public FileController(IFileBusiness fileBusiness)
    {
        _fileBusiness = fileBusiness;
    }

    [HttpGet("downloadFile/{fileName}")]
    [ProducesResponseType((200), Type = typeof(byte[]))]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Produces("application/octet-stream")]

    public async void GetFileAsync(string fileName)
    {
        byte[] buffer = _fileBusiness.GetFile(fileName);
        /*
        ! transformado em 'void', pois já estou mexendo na response, assim se tentar retornar algo, o sistema tenta
        ! mudar novamente a response, mas como já iniciou com o WriteAsync, gera a exceção
        */
        if (buffer != null)
        {
            HttpContext.Response.ContentType = $"application/{Path.GetExtension(fileName).Replace(".", "")}";
            HttpContext.Response.Headers.Append("content-length", buffer.Length.ToString());
            await HttpContext.Response.Body.WriteAsync(buffer, 0, buffer.Length);
        }
    }

    [HttpPost("uploadFile")]
    [ProducesResponseType((200), Type = typeof(FileDetailVO))]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Produces("application/json")]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
    {
        FileDetailVO detail = await _fileBusiness.SaveFileToDisk(file);
        return new OkObjectResult(detail);
    }

    [HttpPost("uploadMultipleFiles")]
    [ProducesResponseType((200), Type = typeof(List<FileDetailVO>))]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Produces("application/json")]
    public async Task<IActionResult> UploadMayFiles([FromForm] List<IFormFile> files)
    {
        List<FileDetailVO> details = await _fileBusiness.SaveFilesToDisk(files);
        return new OkObjectResult(details);
    }

}





/* //original do curso para download de arquivo (que gera erro)
    [HttpGet("downloadFile/{fileName}")]
    [ProducesResponseType((200), Type = typeof(byte[]))]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Produces("application/octet-stream")]
    public async Task<IActionResult> GetFileAsync(string fileName)
    {
        byte[] buffer = _fileBusiness.GetFile(fileName);
        if (buffer != null)
        {
            HttpContext.Response.ContentType =
                $"application/{Path.GetExtension(fileName).Replace(".", "")}";
            HttpContext.Response.Headers.Append("content-length", buffer.Length.ToString());
            await HttpContext.Response.Body.WriteAsync(buffer, 0, buffer.Length);
        }
        return new ContentResult();
    }
// */