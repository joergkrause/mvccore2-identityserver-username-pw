using JoergIsAGeek.Workshop.IdentityServer4.ResourceServer.Filter;
using JoergIsAGeek.Workshop.IdentityServer4.ResourceServer.Middleware;
using JoergIsAGeek.Workshop.IdentityServer4.ResourceServer.Model;
using JoergIsAGeek.Workshop.IdentityServer4.ResourceServer.Repositories;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JoergIsAGeek.Workshop.IdentityServer4.ResourceServer.Controllers
{
  [Authorize]
  [Route("api/[controller]")]
  public class DataController : Controller
  {
    private readonly IDataRepository _dataEventRecordRepository;
    private readonly ILogger _logger;


    public DataController(IDataRepository dataEventRecordRepository, ILoggerFactory loggerFactory)
    {
      _dataEventRecordRepository = dataEventRecordRepository;
      _logger = loggerFactory.CreateLogger("DataEventRecordsController");
    }

    //[Authorize("dataEventRecordsUser")]
    //[AllowAnonymous]
    [MapUserFilter()]
    [HttpGet]
    public IActionResult Get()
    {
      var user = HttpContext.GetCustomUser();
      var userName = HttpContext.User.FindFirst("username")?.Value;
      _logger.LogInformation("User requested all data {UserName}", userName);
      return Ok(new { data = "demo data" });
    }

    [Authorize("dataEventRecordsAdmin")]
    [HttpGet("{id}")]
    public IActionResult Get(long id)
    {
      return Ok(_dataEventRecordRepository.Get(id));
    }

    [Authorize("dataEventRecordsAdmin")]
    [HttpPost]
    public void Post([FromBody]Data value)
    {
      _dataEventRecordRepository.Post(value);
    }

    [Authorize("dataEventRecordsAdmin")]
    [HttpPut("{id}")]
    public void Put(long id, [FromBody]Data value)
    {
      _dataEventRecordRepository.Put(id, value);
    }

    [Authorize("dataEventRecordsAdmin")]
    [HttpDelete("{id}")]
    public void Delete(long id)
    {
      _dataEventRecordRepository.Delete(id);
    }
  }
}
