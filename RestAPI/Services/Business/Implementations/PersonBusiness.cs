using RestAPI.Hypermedia.Utils;
using RestAPI.Services.Repository;

public class PersonBusiness : IPersonBusiness
{
    private readonly IPersonRepository _repository;
    private readonly PersonConverter _converter;

    public PersonBusiness(IPersonRepository repository)
    {
        _repository = repository;
        _converter = new PersonConverter();
    }

    public List<PersonVO> FindAll()
    {
        return _converter.Parse(_repository.FindAll());
    }

    public PagedSearchVO<PersonVO> FindWIthPagedSearch(string name, string sortDirecion, int pageSize, int page)
    {
        var sort = (!string.IsNullOrWhiteSpace(sortDirecion) && !sortDirecion.ToLower().Equals("desc")) ? "asc" : "desc";
        var size = (pageSize < 0) ? 1 : pageSize;
        var offset = page > 0 ? (page - 1) * size : 0;

        string querySearch = @"SELECT * FROM PERSON P WHERE 1 = 1 ";
        if (!string.IsNullOrWhiteSpace(name))
            querySearch += $"AND LOWER(P.FIRST_NAME) LIKE '%{name}% ";
        querySearch += $"ORDER BY P.FIRST_NAME {sort} LIMIT {size} OFFSET {offset}";

        string queryCount = @"SELECT COUNT(*) FROM PERSON P WHERE 1 = 1 ";
        if (!string.IsNullOrWhiteSpace(name))
            querySearch += $"AND LOWER(P.FIRST_NAME) LIKE LOWER('%{name}%') ";

        var persons = _repository.FindWithPagedSearch(querySearch);
        var totalResults = _repository.GetCount(queryCount);
        return new PagedSearchVO<PersonVO>
        {
            CurrentPage = page,
            List = _converter.Parse(persons),
            PageSize = size,
            SortDirections = sort,
            TotalResults = totalResults
        };
    }

    public PersonVO FindById(long id)
    {
        return _converter.Parse(_repository.FindById(id));
    }

    public List<PersonVO> FindBYName(string firstName, string lastName)
    {
        return _converter.Parse(_repository.FindByName(firstName, lastName));
    }

    public PersonVO Create(PersonVO person)
    {
        var personEntity = _converter.Parse(person);
        personEntity = _repository.Create(personEntity);
        return _converter.Parse(personEntity);
    }

    public void Delete(long id)
    {
        _repository.Delete(id);
    }


    public PersonVO Update(PersonVO person)
    {
        var personEntity = _converter.Parse(person);
        personEntity = _repository.Update(personEntity);
        return _converter.Parse(personEntity);
    }

    public PersonVO Disable(long id)
    {
        var personEntity = _repository.Disable(id);
        return _converter.Parse(personEntity);
    }


}