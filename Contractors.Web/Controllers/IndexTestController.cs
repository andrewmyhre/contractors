using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Contractors.Core;
using Contractors.Core.Domain;

namespace Contractors.Web.Controllers
{
    public class IndexTestController : Controller
    {
        private readonly IDbContext _dbContext;

        public IndexTestController(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //
        // GET: /IndexTest/

        public string Index(string q)
        {
            StringBuilder sb = new StringBuilder();

            using (var session = _dbContext.OpenSession() as RavenDbSession)
            {
                var query = session.Session.Advanced
                    .LuceneQuery<Candidate>("candidateSearch");

                if (!string.IsNullOrWhiteSpace(q))
                {

                    query.Where(string.Format("SkillName:\"{0}\" OR FullName:\"{0}\"",q));
                }

                sb.AppendFormat("{0} results<br/>", query.LongCount());

                foreach(var r in query)
                {
                    sb.AppendFormat("{0}, {1}<br/>", r.FullName, r.Skills);
                }
            }

            return sb.ToString();
        }

    }
}
