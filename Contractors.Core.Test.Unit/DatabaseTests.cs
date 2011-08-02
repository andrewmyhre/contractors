using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contractors.Core.Domain;
using NUnit.Framework;

namespace Contractors.Core.Test.Unit
{
    [TestFixture]
    public class DatabaseTests
    {
        [Test]
        public void DbListSession_SaveEntity_EntityCanBeQueried()
        {
            var context = new ListDbContext();
            using (var session = context.OpenSession())
            {
                var candidate = new Candidate();
                candidate.FullName = "test candidate";
                candidate.EmailAddress = "test@test.com";
                session.SaveOrUpdate(candidate);

                var candidate2 =
                    session.Query<Candidate>().Where(c => c.FullName == candidate.FullName).FirstOrDefault();

                Assert.That(candidate2, Is.Not.Null);
            }
        }

        [Test]
        public void DbListSession_SaveEntity_EntityIsAssignedAnId()
        {
            var context = new ListDbContext();
            using (var session = context.OpenSession())
            {
                var candidate = new Candidate();
                candidate.FullName = "test candidate";
                candidate.EmailAddress = "test@test.com";
                session.SaveOrUpdate(candidate);

                var candidate2 =
                    session.Query<Candidate>().Where(c => c.FullName == candidate.FullName).FirstOrDefault();

                Assert.That(candidate2.Id, Is.Not.Null);
            }
        }
    }
}
