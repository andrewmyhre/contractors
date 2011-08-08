﻿using System;
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
select new {skill.SkillName, c.FullName}",
                            Analyzers =
                                {
                                    {"FullName", typeof(SimpleAnalyzer).FullName},
                                    {"SkillName", typeof(SimpleAnalyzer).FullName}
                                }
                        });
            }
        }
    }
}