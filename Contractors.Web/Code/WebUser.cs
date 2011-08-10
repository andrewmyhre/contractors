using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Contractors.Web.Code
{
    public class WebUser : IPrincipal
    {
        private IIdentity _identity;

        public WebUser(string username)
        {
            _identity = new GenericIdentity(username);
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
}