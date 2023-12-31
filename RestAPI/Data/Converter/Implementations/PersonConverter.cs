using RestAPI.Hypermedia;

public class PersonConverter : IParser<PersonVO, Person>, IParser<Person, PersonVO>
{
    public Person Parse(PersonVO origem)
    {
        if (origem == null) return null;
        return new Person(
            origem.Id,
            origem.FirstName,
            origem.LastName,
            origem.Address,
            origem.Gender,
            origem.Enabled
        );
    }
    public PersonVO Parse(Person origem)
    {
        if (origem == null) return null;
        return new PersonVO
        {
            Id = origem.Id,
            FirstName = origem.FirstName,
            LastName = origem.LastName,
            Address = origem.Address,
            Gender = origem.Gender,
            Enabled = origem.Enabled
        };
    }

    public List<Person> Parse(List<PersonVO> origem)
    {
        if (origem == null) return null;
        return origem.Select(item => Parse(item)).ToList();
    }

    public List<PersonVO> Parse(List<Person> origem)
    {
        if (origem == null) return null;
        return origem.Select(item => Parse(item)).ToList();
    }

}