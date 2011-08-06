using System.Linq;
using Raven.Client;

namespace Contractors.Core
{
    public class RavenDbSession : IDbSession
    {
        private readonly IDocumentSession _dbSession;

        public RavenDbSession(IDocumentSession dbSession)
        {
            _dbSession = dbSession;
        }

        public void Dispose()
        {
            _dbSession.Dispose();
        }

        public IQueryable<T> Query<T>()
        {
            return _dbSession.Query<T>();
        }

        public void SaveOrUpdate<T>(T entity)
        {
            _dbSession.Store(entity);
        }

        public void Commit()
        {
            _dbSession.SaveChanges();
        }

        public void Delete<T>(T entity)
        {
            _dbSession.Delete(entity);
        }
    }
}