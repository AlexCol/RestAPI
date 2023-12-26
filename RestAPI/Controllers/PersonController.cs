using Microsoft.AspNetCore.Mvc;
using RestAPI.Hypermedia.Filters;

[ApiVersion("1")]
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
        return Ok(_personBusiness.FindAll());
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
    [HttpDelete("{id}")]
    //!anotação abaixo é para uso do swagger
    [ProducesResponseType(204)]
    public IActionResult Delete(long Id)
    {
        _personBusiness.Delete(Id);
        return NoContent();
    }
}
