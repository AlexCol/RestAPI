using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using RestAPI.Data.VO;
using RestAPI.Model;
using RestAPI.Services.Business;
using Serilog;

namespace RestAPI.Controllers;

[ApiVersion("1")]
[Authorize]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class SavedFileController : ControllerBase
{
    private readonly ISavedFileBusiness _savedFileBusiness;

    public SavedFileController(ISavedFileBusiness savedFileBusiness)
    {
        _savedFileBusiness = savedFileBusiness;
    }

    [HttpGet("downloadFile/{fileName}")]
    [ProducesResponseType((200), Type = typeof(byte[]))]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Produces("application/octet-stream")]
    public async void GetFileAsync(string fileName)
    {
        try
        {
            byte[] buffer = _savedFileBusiness.FindBYName(fileName).FileData;
            if (buffer != null)
            {
                //HttpContext.Response.ContentType = $"application/{Path.GetExtension(fileName).Replace(".", "")}";
                HttpContext.Response.Headers.Append("content-length", buffer.Length.ToString());
                await HttpContext.Response.Body.WriteAsync(buffer, 0, buffer.Length);
            }
        }
        catch (Exception)
        {
            HttpContext.Response.StatusCode = 400;
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
        SavedFile savedFile = await _savedFileBusiness.Create(file, User.Identity.Name);
        return new OkObjectResult(savedFile);
    }

    [HttpPost("uploadMultipleFiles")]
    [ProducesResponseType((200), Type = typeof(List<FileDetailVO>))]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [Produces("application/json")]
    public async Task<IActionResult> UploadMayFiles([FromForm] List<IFormFile> files)
    {
        List<SavedFile> list = new List<SavedFile>();
        foreach (IFormFile file in files)
        {
            list.Add(await _savedFileBusiness.Create(file, User.Identity.Name));
        }
        return new OkObjectResult(list);
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