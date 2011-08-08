using System;

namespace Contractors.Core.Domain
{
    public class Placement
    {
        public string CompanyName { get; set; }
        public CompanySector Sector { get; set; }
        public string CompanySectorName { get { return Enum.GetName(typeof (CompanySector), Sector); } }
        public DateTime Started { get; set; }
        public DateTime Finished { get; set; }
        public bool StillThere { get; set; }
        public bool Startup { get; set; }
        public decimal Remuneration { get; set; }
        public RemunerationPeriod RemunerationPeriod { get; set; }
    }
}