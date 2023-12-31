using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Hypermedia.Filters;
using Serilog;

[ApiVersion("1")]
[Authorize(Policy = "AdminPolicy")] // =>por padrão já barra devido a configuração na program, mas com isso se consegue colocar mais uma trava, para casos de roles especificas
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class PersonController : ControllerBase
{
    private readonly ILogger<PersonController> _logger;
    private IPersonBusiness _personBusiness;

    public PersonController(ILogger<PersonController> logger, IPersonBusiness personBusiness)
    {
        _logger = logger;
        _personBusiness = personBusiness;
    }

    [HttpGet()]
    //!anotação abaixo é para uso do swagger
    [ProducesResponseType((200), Type = typeof(List<PersonVO>))]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [TypeFilter(typeof(HyperMediaFilter))]
    public IActionResult FindAll()
    {
        var pessoas = _personBusiness.FindAll();
        return Ok(pessoas);
    }

    [HttpGet("{sortDirection}/{pageSize}/{page}")]
    //!anotação abaixo é para uso do swagger
    [ProducesResponseType((200), Type = typeof(List<PersonVO>))]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [TypeFilter(typeof(HyperMediaFilter))]
    public IActionResult FindPaged(
        [FromQuery] string name,
        string sortDirection,
        int pageSize,
        int page
    )
    {
        var pessoas = _personBusiness.FindWIthPagedSearch(name, sortDirection, pageSize, page);
        return Ok(pessoas);
    }

    [HttpGet("{id}")]
    //!anotação abaixo é para uso do swagger
    [ProducesResponseType((200), Type = typeof(PersonVO))]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [TypeFilter(typeof(HyperMediaFilter))]
    public IActionResult FindById(long id)
    {
        var person = _personBusiness.FindById(id);
        return person == null ? NotFound() : Ok(person);
    }

    [HttpGet("findPersonByName")]
    //!anotação abaixo é para uso do swagger
    [ProducesResponseType((200), Type = typeof(PersonVO))]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [TypeFilter(typeof(HyperMediaFilter))]
    public IActionResult FindByName([FromQuery] string firstName, [FromQuery] string lastName)
    {
        var person = _personBusiness.FindBYName(firstName, lastName);
        return person == null ? NotFound() : Ok(person);
    }

    [HttpPost()]
    //!anotação abaixo é para uso do swagger
    [ProducesResponseType((200), Type = typeof(PersonVO))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [TypeFilter(typeof(HyperMediaFilter))]
    public IActionResult Create([FromBody] PersonVO personBody)
    {
        var personCreated = _personBusiness.Create(personBody);
        return Created(
            "Person/" + personCreated.Id,
            personCreated
        );
    }

    [HttpPut()]
    //!anotação abaixo é para uso do swagger
    [ProducesResponseType((200), Type = typeof(PersonVO))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [TypeFilter(typeof(HyperMediaFilter))]
    public IActionResult Update([FromBody] PersonVO personBody)
    {
        if (personBody == null)
        {
            return BadRequest("Not a valid person");
        }
        var person = _personBusiness.Update(personBody);
        return Ok(person);
    }

    [HttpPatch("{id}")]
    //!anotação abaixo é para uso do swagger
    [ProducesResponseType((200), Type = typeof(PersonVO))]
    [TypeFilter(typeof(HyperMediaFilter))]
    public IActionResult Disable(long id)
    {
        var person = _personBusiness.Disable(id);
        return person == null ? NotFound() : Ok(person);
    }

    [HttpDelete("{id}")]
    //!anotação abaixo é para uso do swagger
    [ProducesResponseType(204)]
    public IActionResult Delete(long Id)
    {
        _personBusiness.Delete(Id);
        return NoContent();
    }
}
