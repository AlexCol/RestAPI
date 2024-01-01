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
}
