using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Contractors.Web.Controllers;

namespace Contractors.Web.Code
{
    public static class RequestExtensions
    {
        public static ErrorsResponse ToResponse(this ModelStateDictionary modelState)
        {
            ErrorsResponse response = new ErrorsResponse(){Errors = new List<ErrorInformation>()};
            var errors = modelState.Where(ms => ms.Value.Errors.Count > 0);
            foreach (var error in errors)
            {
                response.Errors.Add(new ErrorInformation(){Key=error.Key, Messages = error.Value.Errors.Select(e=>e.ErrorMessage).ToArray()});
            }
            return response;
        }
    }
}