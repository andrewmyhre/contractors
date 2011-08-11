using System;
using System.ComponentModel.DataAnnotations;

namespace Contractors.Web.Models
{
    public class SignUpRequest
    {
        [Required(ErrorMessage="Please provide a first name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please provide a last name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please provide an email address")]
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}