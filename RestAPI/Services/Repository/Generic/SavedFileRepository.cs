using RestAPI.Model;

namespace RestAPI.Services.Repository;

public class SavedFileRepository : GenericRepository<SavedFile>, ISavedFileRepository
{
    public SavedFileRepository(MySqlContext context) : base(context) { }


    public SavedFile FindByName(string fileName)
    {
        var validFileName = !string.IsNullOrWhiteSpace(fileName);

        if (!validFileName)
            return null;

        return _context.SavedFiles.FirstOrDefault(sf => sf.FileName.Equals(fileName));
    }
}
