using System;
using System.Linq;
using Contractors.Core.Domain;

namespace Contractors.Core
{
    public interface IDbSession : IDisposable
    {
        IQueryable<T> Query<T>();
        void SaveOrUpdate<T>(T entity);
        void Commit();
        void Delete<T>(T entity);
    }
}