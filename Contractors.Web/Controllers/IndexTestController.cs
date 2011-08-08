using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Contractors.Core;
using Contractors.Core.Domain;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Tokenattributes;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Util;
using Raven.Client;

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

        private static string[] DefaultFields
        {
            get
            {
                return new[]
                           {
                               "FullName",
                               "SkillName",
                               "CompanyName",
                               "Sector"
                           };
            }
        }

        public string Index(string q, string[] fields)
        {
            StringBuilder sb = new StringBuilder();
            
            using (var session = _dbContext.OpenSession() as RavenDbSession)
            {
                if (fields == null || fields.Length==0)
                    fields = DefaultFields.ToArray();

                IDocumentQuery<Candidate> query = CreateQuery<Candidate>(session, "candidateSearch", q, fields);

                sb.AppendFormat("<p>{0}</p>", query.ToString());
                sb.AppendFormat("{0} results<br/>", query.LongCount());

                foreach(var r in query)
                {
                    var score = session.Session.Advanced.GetMetadataFor(r)["Temp-Index-Score"];
                    sb.AppendFormat("({0:#}%)", score);
                    sb.Append(r.FullName);
                    sb.Append(": " + string.Join(", ", r.Skills.Select(s => s.SkillName).ToArray()));
                    sb.Append(": " + string.Join(", ", r.WorkHistory.Select(p => p.CompanyName).ToArray()));
                    sb.Append("<br/>");
                }
            }

            return sb.ToString();
        }

        public ActionResult Search(string q)
        {
            List<SearchResult> results = new List<SearchResult>();
            using (var session = _dbContext.OpenSession() as RavenDbSession)
            {
                IDocumentQuery<Candidate> query = session.Session.Advanced.LuceneQuery<Candidate>().Where(q);

                
                foreach (var r in query)
                {
                    var score = session.Session.Advanced.GetMetadataFor(r)["Temp-Index-Score"];
                    results.Add(new SearchResult() { Candidate = r, Score = double.Parse(score.ToString()) });
                }
            }

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Advanced()
        {
            return View();
        }

        public ActionResult AdvancedSearch(string[] q, string[] fields)
        {
            if (q != null && fields != null && q.Length != fields.Length)
                throw new ArgumentException("query length is different to fields length");

            StringBuilder sb = new StringBuilder();
            List<SearchResult> results = new List<SearchResult>();

            using (var session = _dbContext.OpenSession() as RavenDbSession)
            {
                IDocumentQuery<Candidate> query = session.Session.Advanced.LuceneQuery<Candidate>("candidateSearch");


                if (q != null && fields != null)
                {
                    for(int i=0;i<fields.Length;i++)
                    {
                        query.WhereContains(fields[i], q[i]);
                    }
                }

                sb.AppendFormat("<p>{0}</p>", query.ToString());
                sb.AppendFormat("{0} results<br/>", query.LongCount());

                foreach (var r in query)
                {
                    var score = session.Session.Advanced.GetMetadataFor(r)["Temp-Index-Score"];
                    results.Add(new SearchResult(){Candidate = r, Score = double.Parse(score.ToString())});
                    
                    sb.AppendFormat("({0:#}%)", score);
                    sb.Append(r.FullName);
                    sb.Append(": " + string.Join(", ", r.Skills.Select(s => s.SkillName).ToArray()));
                    sb.Append(": " + string.Join(", ", r.WorkHistory.Select(p => p.CompanyName).ToArray()));
                    sb.Append("<br/>");
                }
            }

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        private IDocumentQuery<T> CreateQuery<T>(RavenDbSession session, string indexName, string q, string[] fields)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return session.Session.Advanced
                .LuceneQuery<T>(indexName);
            }

            StringReader reader = new StringReader(q);
            var tokenizer = new LowerCaseTokenizer(AttributeSource.AttributeFactory.DEFAULT_ATTRIBUTE_FACTORY, reader);

            var query = session.Session.Advanced
                .LuceneQuery<T>(indexName);

            while (tokenizer.IncrementToken())
            {
                var attr = tokenizer.GetAttribute(typeof (TermAttribute)) as TermAttribute;
                foreach (string field in fields)
                    query.WhereContains(field, attr.Term()).Fuzzy(0.6m);
            }
            return query;
        }

        private IEnumerable<string> ExtractTerms(string q)
        {
            StringReader reader = new StringReader(q);
            var tokenizer = new LowerCaseTokenizer(AttributeSource.AttributeFactory.DEFAULT_ATTRIBUTE_FACTORY, reader);

            List<string> terms = new List<string>();
            while (tokenizer.IncrementToken())
            {
                var attr = tokenizer.GetAttribute(typeof(TermAttribute)) as TermAttribute;
                terms.Add(attr.Term());
            }
            return terms;
        }
    }

    public class SearchResult
    {
        public Candidate Candidate { get; set; }
        public double Score { get; set; }
    }
}
