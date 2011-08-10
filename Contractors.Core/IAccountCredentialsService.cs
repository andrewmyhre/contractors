using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contractors.Core.Domain;

namespace Contractors.Core
{
    public interface IAccountCredentialsService
    {
        bool ValidatePassword(string credentialsId, string passwordAttempt);
        AccountCredentials CreateCredentials(string password);
        AccountCredentials CreateCredentials(IDbSession dbSession, string password);
        void UpdatePassword(string credentialsId, string password);
    }
}
