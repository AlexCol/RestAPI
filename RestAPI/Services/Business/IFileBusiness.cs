using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestAPI.Data.VO;

namespace RestAPI.Services.Business;

public interface IFileBusiness
{
    public byte[] GetFile(string fileName);
    public Task<FileDetailVO> SaveFileToDisk(IFormFile file);
    public Task<List<FileDetailVO>> SaveFilesToDisk(List<IFormFile> files);

}
