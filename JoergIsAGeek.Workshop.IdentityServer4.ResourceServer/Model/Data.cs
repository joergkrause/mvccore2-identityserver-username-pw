using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JoergIsAGeek.Workshop.IdentityServer4.ResourceServer.Model
{

    public class Data
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
