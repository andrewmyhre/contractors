using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Contractors.Web.Models
{
    public class SignUpRequest
    {
        [Required(ErrorMessage="Please provide a first name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please provide a last name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please provide an email address")]
        [LowerCaseRegularExpression(@"^[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$", ErrorMessage="Please provide a valid email address")]
        public string EmailAddress { get; set; }
        [StringLength(9999, MinimumLength = 6, ErrorMessage = "Please provide a password at least 6 characters long")]
        [Required(ErrorMessage = "Please provide a password")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage="Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}