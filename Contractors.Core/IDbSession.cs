using System;
using System.Linq;

namespace Contractors.Core
{
    public interface IDbSession : IDisposable
    {
        IQueryable<T> Query<T>();
        void SaveOrUpdate<T>(T entity);
        void Commit();
    }
}