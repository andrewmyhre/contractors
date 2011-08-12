using System;
using System.Collections.Generic;
using System.Text;

namespace Contractors.Core.Domain
{
    public class UserAccount
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string CredentialsId { get; set; }

    }
}