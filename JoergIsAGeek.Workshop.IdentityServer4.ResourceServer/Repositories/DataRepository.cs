using System;
using System.Collections.Generic;
using System.Linq;
using JoergIsAGeek.Workshop.IdentityServer4.ResourceServer.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JoergIsAGeek.Workshop.IdentityServer4.ResourceServer.Repositories
{
  public class DataRepository : IDataRepository
  {
    private readonly ILogger _logger;

    private readonly IEnumerable<Data> _data = new List<Data> {
      new Data { Id = 1, Name = "Record One", Description = "Record number 1", Timestamp = DateTime.Now },
      new Data { Id = 2, Name = "Record Two", Description = "Record number 2", Timestamp = DateTime.Now },
      new Data { Id = 3, Name = "Record Three", Description = "Record number 3", Timestamp = DateTime.Now },
      new Data { Id = 4, Name = "Record Four", Description = "Record number 4", Timestamp = DateTime.Now },
    };

    public DataRepository(ILoggerFactory loggerFactory)
    {
      _logger = loggerFactory.CreateLogger("IDataRepository");
    }

    public List<Data> GetAll()
    {
      _logger.LogInformation("Getting all existing records");
      return _data.ToList();
    }

    public Data Get(long id)
    {
      var dataEventRecord = _data.First(t => t.Id == id);
      return dataEventRecord;
    }

    [HttpPost]
    public void Post(Data dataEventRecord)
    {
      _data.ToList().Add(dataEventRecord);
    }

    public void Put(long id, [FromBody]Data dataEventRecord)
    {
      _data.Single(d => d.Id == id).Name = dataEventRecord.Name;
      _data.Single(d => d.Id == id).Description = dataEventRecord.Description;
      _data.Single(d => d.Id == id).Timestamp = dataEventRecord.Timestamp;
    }

    public void Delete(long id)
    {
      var entity = _data.Single(t => t.Id == id);
      _data.ToList().Remove(entity);
    }
  }
}
