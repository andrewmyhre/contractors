using Contractors.Core.Domain;

namespace Contractors.Core
{
    public interface IUserAccountService
    {
        UserAccount Create(string firstName, string lastName, string emailAddress, string password, string confirmPassword);
        bool CredentialsValid(string emailAddress, string passwordAttempt);
        void UpdateAccount(string firstName, string lastName, string emailAddress);
    }
}