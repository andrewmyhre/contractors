using System;
using System.Linq;
using Contractors.Core.Domain;

namespace Contractors.Core
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IAccountCredentialsService _accountCredentialsService;
        private readonly IDbContext _dbContext;

        public UserAccountService(IAccountCredentialsService accountCredentialsService, IDbContext dbContext)
        {
            _accountCredentialsService = accountCredentialsService;
            _dbContext = dbContext;
        }

        public UserAccount Create(string firstName, string lastName, string emailAddress, string password, string confirmPassword)
        {
            UserAccount account = null;
            using (var session = _dbContext.OpenSession())
            {
                var credentials = _accountCredentialsService.CreateCredentials(session, password);
                session.SaveOrUpdate(credentials);

                account = new UserAccount()
                              {
                                  CredentialsId = credentials.Id,
                                  EmailAddress = emailAddress,
                                  FirstName = firstName,
                                  LastName = lastName
                              };
                session.SaveOrUpdate(account);
                session.Commit();
            }

            return account;
        }

        public bool CredentialsValid(string emailAddress, string passwordAttempt)
        {
            string credentialsId = "";
            using (var session = _dbContext.OpenSession())
            {
                var account = session.Query<UserAccount>().Where(a => a.EmailAddress == emailAddress).FirstOrDefault();
                if (account == null) return false;
                credentialsId = account.CredentialsId;
            }

            return _accountCredentialsService.ValidatePassword(credentialsId, passwordAttempt);
        }

        public void UpdateAccount(string currentEmailAddress, string firstName, string lastName, string newEmailAddress)
        {
            using (var session = _dbContext.OpenSession())
            {
                var account = session.Query<UserAccount>().Where(a => a.EmailAddress == currentEmailAddress).FirstOrDefault();
                account.FirstName = firstName;
                account.LastName = lastName;
                account.EmailAddress = newEmailAddress;
                session.SaveOrUpdate(account);
                session.Commit();
            }
        }

        public UserAccount Retrieve(string emailAddress)
        {
            using (var session = _dbContext.OpenSession())
            {
                return session.Query<UserAccount>().Where(a => a.EmailAddress == emailAddress).FirstOrDefault();
            }
        }
    }
}