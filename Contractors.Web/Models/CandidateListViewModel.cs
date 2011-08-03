using System;
using System.Collections.Generic;
using System.Linq;
using Contractors.Core.Domain;

namespace Contractors.Web.Models
{
    public class CandidateListViewModel
    {
        public IQueryable<Candidate> Candidates { get; set; }

        public DateTime CalendarMonthStartDate { get; set; }

        public int CandidatesNextMonth { get; set; }

        public int CandidatesLastMonth { get; set; }

        public DateTime CalendarMonthEndDate { get; set; }

        public Dictionary<string, IEnumerable<Candidate>> CandidatesByDay { get; set; }

        public DateTime CalendarActualStartDate { get; set; }

        public DateTime CalendarActualEndDate { get; set; }
    }
}