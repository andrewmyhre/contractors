using System;
using Contractors.Core.Domain;

namespace Contractors.Core
{
    public class AccountCredentialsService : IAccountCredentialsService
    {
        private readonly IDbContext _dbContext;

        public AccountCredentialsService(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool ValidatePassword(string credentialsId, string passwordAttempt)
        {
            using (var session = _dbContext.OpenSession())
            {
                var credentials = session.Load<AccountCredentials>(credentialsId);

                return BCrypt.CheckPassword(passwordAttempt, credentials.Password);
            }
        }

        public AccountCredentials CreateCredentials(string password)
        {
            AccountCredentials credentials = null;
            using (var session = _dbContext.OpenSession())
            {
                credentials = new AccountCredentials()
                                  {DateUpdated=DateTime.Now, Password=BCrypt.HashPassword(password, BCrypt.GenerateSalt())};
                session.SaveOrUpdate(credentials);
                session.Commit();
            }
            return credentials;
        }

        public AccountCredentials CreateCredentials(IDbSession dbSession, string password)
        {
            var credentials = new AccountCredentials() { DateUpdated = DateTime.Now, Password = BCrypt.HashPassword(password, BCrypt.GenerateSalt()) };
            dbSession.SaveOrUpdate(credentials);
            return credentials;
        }

        public void UpdatePassword(string credentialsId, string password)
        {
            using (var session = _dbContext.OpenSession())
            {
                var credentials = session.Load<AccountCredentials>(credentialsId);

                credentials.Password = BCrypt.HashPassword(password, BCrypt.GenerateSalt());
            }
        }
    }
}