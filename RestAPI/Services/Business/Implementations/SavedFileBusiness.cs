using RestAPI.Hypermedia.Utils;
using RestAPI.Model;
using RestAPI.Services.Repository;

public class SavedFileBusiness : ISavedFileBusiness
{
    private ISavedFileRepository _repository;
    public SavedFileBusiness(ISavedFileRepository repository)
    {
        _repository = repository;
    }

    public async Task<SavedFile> Create(IFormFile file, string createdBy)
    {
        SavedFile newFile = null;
        var fileType = Path.GetExtension(file.FileName);

        newFile = FindBYName(file.FileName);
        if (newFile != null)
            return await Update(file, createdBy);

        await using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var fileBytes = memoryStream.ToArray();
        newFile = new SavedFile
        {
            FileName = file.FileName,
            CreatedBy = createdBy,
            FileData = fileBytes
        };
        return _repository.Create(newFile);
    }

    public SavedFile FindBYName(string fileName)
    {
        return _repository.FindByName(fileName);
    }

    public async Task<SavedFile> Update(IFormFile file, string createdBy)
    {
        await using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var fileBytes = memoryStream.ToArray();

        var newFile = _repository.FindByName(file.FileName);
        newFile.FileData = fileBytes;

        return _repository.Update(newFile);
    }

}
