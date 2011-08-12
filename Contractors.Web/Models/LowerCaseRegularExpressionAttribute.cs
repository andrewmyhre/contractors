using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Contractors.Web.Models
{
    public class LowerCaseRegularExpressionAttribute : ValidationAttribute
    {
        private Regex _regex;

        public LowerCaseRegularExpressionAttribute(string regex)
        {
            _regex = new System.Text.RegularExpressions.Regex(regex,RegexOptions.IgnoreCase);
        }

        public override bool IsValid(object value)
        {
            return _regex.IsMatch(value.ToString());
        }
    }
}