using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Contractors.Core.Domain
{
    public class AccountCredentials
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
