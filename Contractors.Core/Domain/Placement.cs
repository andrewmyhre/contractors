using System;
using System.Collections.Generic;

namespace Contractors.Core.Domain
{
    public class Placement
    {
        public Placement()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string CompanyName { get; set; }
        public CompanySector Sector { get; set; }
        public string CompanySectorName { get { return Enum.GetName(typeof (CompanySector), Sector); } }
        public DateTime Started { get; set; }
        public DateTime Finished { get; set; }
        public bool StillThere { get; set; }
        public bool Startup { get; set; }
        public decimal Remuneration { get; set; }
        public PlacementType PlacementType { get; set; }
        public RemunerationPeriod RemunerationPeriod { get; set; }
        public IEnumerable<Skill> Skills { get; set; }
        public TimeSpan Duration
        {
            get { return StillThere ? DateTime.Now.Subtract(Started) : Finished.Subtract(Started); }
        }
        public double DurationInYears
        {
            get { return Duration.TotalDays/365; }
        }
    }

    public enum PlacementType
    {
        Permanent,
        Contract,
        Internship
    }
}