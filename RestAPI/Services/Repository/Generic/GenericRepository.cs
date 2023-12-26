using Microsoft.EntityFrameworkCore;

public class GenericRepository<T> : IRepository<T> where T : BaseEntity
{
    private MySqlContext _context;
    private DbSet<T> dataset;

    public GenericRepository(MySqlContext context)
    {
        _context = context;
        dataset = context.Set<T>();
    }

    public T Create(T item)
    {
        try
        {
            dataset.Add(item);
            _context.SaveChanges();
            return item;
        }
        catch (Exception e)
        {
            throw new InvalidDataException("Erro ao cadastrar o item. " + e.Message);
        }
    }

    public void Delete(long id)
    {
        dataset.Remove(FindById(id));
        _context.SaveChanges();
    }

    public bool Exists(long id)
    {
        return dataset.Any(p => p.Id == id);
    }

    public List<T> FindAll()
    {
        return dataset.ToList();
    }

    public T FindById(long id)
    {
        T item = dataset.SingleOrDefault(p => p.Id.Equals(id));
        return item;
    }

    public T Update(T item)
    {
        if (!Exists(item.Id)) throw new InvalidDataException("Erro ao atualizar pessoa o registro.");
        T itemAtual = dataset.SingleOrDefault(p => p.Id.Equals(item.Id));
        if (itemAtual == null) throw new InvalidDataException("Erro ao atualizar o registro.");

        _context.Entry(itemAtual).CurrentValues.SetValues(item);
        _context.SaveChanges();
        return item;
    }
}
