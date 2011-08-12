using System;
using System.Collections.Generic;
using System.Linq;

namespace Contractors.Core.Domain
{
    public class Candidate
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNumber { get; set; }
        public string EmailMd5Hash { get; set; }
        public string LookingFor { get; set; }
        public string Avoiding { get; set; }
        public decimal DesiredRate { get; set; }
        public RemunerationPeriod DesiredRatePeriod { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Placement> WorkHistory { get; set; }
        public string JobTitle { get; set; }
        public int ContractLengthInMonths { get; set; }

        public DateTime AvailableDate
        {
            get
            {
                if (WorkHistory == null || WorkHistory.Count == 0) return DateTime.Now;
                return WorkHistory
                    .Max(h => h.Started)
                    .AddMonths(ContractLengthInMonths);
            }
        }

        public Placement MostRecentRole
        {
            get
            {
                if (WorkHistory == null || WorkHistory.Count == 0) return null;
                return WorkHistory.OrderByDescending(p => p.Started).FirstOrDefault();
            }
        }
        public double TotalExperienceInYears
        {
            get
            {
                if (WorkHistory == null || WorkHistory.Count == 0) return 0;
                return (MostRecentRole.StillThere ? DateTime.Now : MostRecentRole.Finished)
                           .Subtract(WorkHistory.Min(p=>p.Started))
                           .TotalDays/365;
            }
        }
    }
}
