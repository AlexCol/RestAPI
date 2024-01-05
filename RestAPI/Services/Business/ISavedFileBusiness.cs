using RestAPI.Hypermedia.Utils;
using RestAPI.Model;

public interface ISavedFileBusiness
{
    Task<SavedFile> Create(IFormFile file, string createdBy);
    SavedFile FindBYName(string fileName);
    Task<SavedFile> Update(IFormFile file, string createdBy);

}
