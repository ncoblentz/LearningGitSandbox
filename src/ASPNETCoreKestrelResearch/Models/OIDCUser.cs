using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreKestrelResearch.Models
{
    public class OIDCUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
    }
}
