using System.Collections;
using System.Collections.Generic;
using System.Text;
using Contractors.Core.Domain;

namespace Contractors.Core
{
    public class CandidateService
    {
        private readonly IDbContext _dbContext;

        public CandidateService(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Candidate> ListAll()
        {
            using (var session = _dbContext.OpenSession())
            {
                return session.Query<Candidate>();
            }
        }
    }
}
