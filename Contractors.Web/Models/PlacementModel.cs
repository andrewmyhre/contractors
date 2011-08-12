using System;
using System.ComponentModel.DataAnnotations;
using Contractors.Core.Domain;

namespace Contractors.Web.Models
{
    public class PlacementModel
    {
        [Required(ErrorMessage = "Please provide the name of the company")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Please provide company sector")]
        public CompanySector Sector { get; set; }
        [Required(ErrorMessage = "Please provide the date you started", AllowEmptyStrings = false)]
        public DateTime Started { get; set; }
        public DateTime? Finished { get; set; }
        public bool StillThere { get; set; }
        public bool Startup { get; set; }
        public decimal Remuneration { get; set; }
        [Required(ErrorMessage = "Please indicate whether this was a permanent, contract or internship role")]
        public PlacementType PlacementType { get; set; }
        public RemunerationPeriod RemunerationPeriod { get; set; }
        public string SkillSet { get; set; }
    }
}