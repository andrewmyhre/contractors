using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Contractors.Core.Domain;

namespace Contractors.Web.Code
{
    public class WebUser : IPrincipal
    {
        private IIdentity _identity;

        public WebUser(UserAccount account)
        {
            _identity = new ContractorsIdentity(account);
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public IIdentity Identity
        {
            get { return _identity; }
        }
    }

    public class ContractorsIdentity : IIdentity
    {
        private readonly UserAccount _account;

        public ContractorsIdentity(UserAccount account)
        {
            _account = account;
        }

        public string Name
        {
            get { return string.Format("{0} {1}", _account.FirstName, _account.LastName); }
        }

        public string AuthenticationType
        {
            get { return "Forms"; }
        }

        public bool IsAuthenticated
        {
            get { return _account != null; }
        }
    }
}