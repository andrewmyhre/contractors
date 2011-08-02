using System.Linq;
using Contractors.Core.Domain;

namespace Contractors.Web.Models
{
    public class CandidateListViewModel
    {
        public IQueryable<Candidate> Candidates { get; set; }
    }
}