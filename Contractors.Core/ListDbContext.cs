using System;
using System.Collections;
using System.Collections.Generic;

namespace Contractors.Core
{
    public class ListDbContext : IDbContext
    {
        internal Dictionary<Type, IList> Queryables = new Dictionary<Type, IList>();
        public IDbSession OpenSession()
        {
            return new ListDbSession(this);
        }

        public void Dispose()
        {
            // nothing to do
        }
    }
}