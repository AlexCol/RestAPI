using RestAPI.Hypermedia.Utils;

public interface IPersonBusiness
{
    PersonVO Create(PersonVO person);
    PersonVO FindById(long id);
    List<PersonVO> FindBYName(string firstName, string lastName);
    List<PersonVO> FindAll();
    PagedSearchVO<PersonVO> FindWIthPagedSearch(string name, string sortDirecion, int pageSize, int page);
    PersonVO Update(PersonVO person);
    PersonVO Disable(long id);
    void Delete(long id);

}
