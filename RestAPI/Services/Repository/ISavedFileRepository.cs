using RestAPI.Model;

namespace RestAPI.Services.Repository;

public interface ISavedFileRepository : IRepository<SavedFile>
{
    SavedFile FindByName(string fileName);
}
