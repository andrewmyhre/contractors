using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Contractors.Web.Code
{
    public static class CalendarExtensions
    {
        public static MvcHtmlString EndWeekRow(this HtmlHelper html, DateTime day)
        {
            if (day.DayOfWeek == DayOfWeek.Sunday)
                return new MvcHtmlString(@"</tr></tbody></table></div><div class=""month-row""><table><tbody><tr>");

            return new MvcHtmlString("");
        }
    }
}