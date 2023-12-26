public class PersonRepository : IPersonRepository
{
    private MySqlContext _context;

    public PersonRepository(MySqlContext context)
    {
        _context = context;
    }

    public Person Create(Person personRequest)
    {
        try
        {
            _context.Add(personRequest);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new InvalidDataException("Erro ao cadastrar a pessoa. " + e.Message);
        }
        return personRequest;
    }

    public void Delete(long id)
    {
        _context.Persons.Remove(FindById(id));
        _context.SaveChanges();
    }

    public Person FindById(long id)
    {
        Person novaPerson = _context.Persons.SingleOrDefault(p => p.Id.Equals(id));
        return novaPerson;
    }

    public List<Person> FindAll()
    {
        return _context.Persons.ToList();
    }

    public Person Update(Person person)
    {
        if (!Exists(person.Id)) throw new InvalidDataException("Erro ao atualizar pessoa a pessoa.");
        Person personAtual = _context.Persons.SingleOrDefault(p => p.Id.Equals(person.Id));
        if (personAtual == null) throw new InvalidDataException("Erro ao atualizar a pessoa.");

        _context.Entry(personAtual).CurrentValues.SetValues(person);
        _context.SaveChanges();
        return person;
    }

    public bool Exists(long id)
    {
        return _context.Persons.Any(p => p.Id == id);
    }
}