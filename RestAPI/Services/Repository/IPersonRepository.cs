using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Services.Repository;

public interface IPersonRepository : IRepository<Person>
{
    Person Disable(long id);
}
