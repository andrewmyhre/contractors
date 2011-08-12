using System.ComponentModel.DataAnnotations;

namespace Contractors.Web.Models
{
    public class SignInRequest
    {
        [Required(ErrorMessage="Please enter an email address to sign in")]
        [LowerCaseRegularExpression(@"^[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$", ErrorMessage = "Please enter a valid email addressto sign in")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Please enter a password to sign in")]
        public string Password { get; set; }
    }
}