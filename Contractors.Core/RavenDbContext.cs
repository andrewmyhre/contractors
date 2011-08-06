using Raven.Client;

namespace Contractors.Core
{
    public class RavenDbContext : IDbContext
    {
        private readonly IDocumentStore _dbStore;

        public RavenDbContext(IDocumentStore dbStore)
        {
            _dbStore = dbStore;
        }

        public void Dispose()
        {
            // nothing to do
        }

        public IDbSession OpenSession()
        {
            return new RavenDbSession(_dbStore.OpenSession());
        }
    }
}