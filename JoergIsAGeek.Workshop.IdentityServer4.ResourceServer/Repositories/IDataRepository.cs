using System.Collections.Generic;
using JoergIsAGeek.Workshop.IdentityServer4.ResourceServer.Model;
using Microsoft.AspNetCore.Mvc;

namespace JoergIsAGeek.Workshop.IdentityServer4.ResourceServer.Repositories
{
    public interface IDataRepository
    {
        void Delete(long id);
        Data Get(long id);
        List<Data> GetAll();
        void Post(Data dataEventRecord);
        void Put(long id, [FromBody] Data dataEventRecord);
    }
}