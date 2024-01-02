using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestAPI.Data.VO;

namespace RestAPI.Services.Business.Implementations;

public class FileBusiness : IFileBusiness
{
    private readonly string _basePath;
    private readonly IHttpContextAccessor _context;

    public FileBusiness(IHttpContextAccessor context)
    {
        _context = context;
        _basePath = Directory.GetCurrentDirectory() + "\\UploadDir\\";
    }

    public byte[] GetFile(string fileName)
    {
        var filePath = _basePath + fileName;
        return File.ReadAllBytes(filePath);
    }

    public async Task<FileDetailVO> SaveFileToDisk(IFormFile file)
    {
        if (file == null || file.Length <= 0)
        {
            return null;
        }

        FileDetailVO fileDetail = new FileDetailVO();

        var fileType = Path.GetExtension(file.FileName);
        var baseUrl = _context.HttpContext.Request.Host;

        if (
            fileType.ToLower() == ".pdf"
            || fileType.ToLower() == ".jpg"
            || fileType.ToLower() == ".png"
            || fileType.ToLower() == ".jpeg"
            || fileType.ToLower() == ".rar"
            || fileType.ToLower() == ".zip"
        )
        {
            var docName = Path.GetFileName(file.FileName);
            var destination = Path.Combine(_basePath, "", docName);
            fileDetail.DocumentName = docName;
            fileDetail.DocType = fileType;
            fileDetail.DocUrl = Path.Combine(baseUrl + "/api/v1/file" + fileDetail.DocumentName);

            //* para salvar o arquivo em banco, esse trecho aqui que precisaria ajustar (pesquisar como fazer)
            using var stream = new FileStream(destination, FileMode.Create);
            await file.CopyToAsync(stream);
        }

        return fileDetail;
    }

    public async Task<List<FileDetailVO>> SaveFilesToDisk(List<IFormFile> files)
    {
        List<FileDetailVO> fileDetails = new List<FileDetailVO>();
        foreach (var file in files)
        {
            fileDetails.Add(await SaveFileToDisk(file));
        }
        return fileDetails;
    }
}
