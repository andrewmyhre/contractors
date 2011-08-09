using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lucene.Net.Analysis;
using Raven.Abstractions.Indexing;
using Raven.Client;

namespace Contractors.Web.Code
{
    public class Indexes
    {
        public static void InitialiseIndexes(IDocumentStore ravenDocumentStore)
        {
            if (ravenDocumentStore.DatabaseCommands.GetIndex("candidateSearch")==null)
            {
                ravenDocumentStore.DatabaseCommands.DeleteIndex("candidateSearch");

                ravenDocumentStore.DatabaseCommands.PutIndex("candidateSearch",
                    new IndexDefinition()
                        {
                            Map = @"from c in docs.Candidates 
from skill in Hierarchy(c, ""Skills"")
from placement in Hierarchy(c, ""WorkHistory"")
select new {skill.SkillName, c.FullName, placement.CompanyName, placement.Sector}",
                            Analyzers =
                                {
                                    {"FullName", typeof(StopAnalyzer).FullName},
                                    {"SkillName", typeof(StopAnalyzer).FullName},
                                    {"CompanyName", typeof(StopAnalyzer).FullName},
                                    {"Sector", typeof(StopAnalyzer).FullName}
                                }
                        });
            }
        }
    }
}