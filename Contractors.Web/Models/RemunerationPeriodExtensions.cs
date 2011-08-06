using Contractors.Core.Domain;

namespace Contractors.Web.Models
{
    public static class RemunerationPeriodExtensions
    {
        public static string ToPeriodString(this RemunerationPeriod period)
        {
            switch(period)
            {
                case RemunerationPeriod.PerDay:
                    return "per day";
                case RemunerationPeriod.PerWeek:
                    return "per week";
                case RemunerationPeriod.PerMonth:
                    return "per month";
                case RemunerationPeriod.PerYear:
                    return "per year";
            }
            return period.ToString();
        }
    }
}