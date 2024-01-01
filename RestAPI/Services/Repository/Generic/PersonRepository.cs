
namespace RestAPI.Services.Repository.Generic;

public class PersonRepository : GenericRepository<Person>, IPersonRepository
{
    public PersonRepository(MySqlContext context) : base(context) { }

    public Person Disable(long id)
    {
        if (!_context.Persons.Any(p => p.Id == id))
            return null;

        var user = _context.Persons.Single(p => p.Id == id);
        user.Enabled = false;
        _context.Entry(user).CurrentValues.SetValues(user);
        _context.SaveChanges();
        return user;
    }

    public List<Person> FindByName(string firstName, string lastName)
    {
        var validFirstName = !string.IsNullOrWhiteSpace(firstName);
        var validLastName = !string.IsNullOrWhiteSpace(lastName);

        if (!validFirstName && !validLastName)
            return null;

        if (validFirstName && !validLastName)
            return _context.Persons.Where(p => p.FirstName.Contains(firstName)).ToList();

        if (!validFirstName && validLastName)
            return _context.Persons.Where(p => p.LastName.Contains(lastName)).ToList();

        return _context.Persons.Where(p => p.FirstName.Contains(firstName) && p.LastName.Contains(lastName)).ToList();
    }
}
