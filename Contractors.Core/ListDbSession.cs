using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Contractors.Core
{
    public class ListDbSession : IDbSession
    {
        private readonly ListDbContext _listDbContext;

        public ListDbSession(ListDbContext listDbContext)
        {
            _listDbContext = listDbContext;
        }

        public void Dispose()
        {
            // nothing to do
        }

        public IQueryable<T> Query<T>()
        {
            if (!_listDbContext.Queryables.ContainsKey(typeof(T)))
            {
                _listDbContext.Queryables.Add(typeof(T), new List<T>());
            }

            return _listDbContext.Queryables[typeof(T)].Cast<T>().AsQueryable();
        }

        public void SaveOrUpdate<T>(T entity)
        {

            if (!_listDbContext.Queryables.ContainsKey(typeof(T)))
            {
                _listDbContext.Queryables.Add(typeof(T), new List<T>());
            }
            var repo = _listDbContext.Queryables[typeof(T)];


            var idProperties = typeof (T).GetProperties();
            var idProperty = idProperties.Where(p => string.Equals(p.Name, "id", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            int nextIdValue = repo.Count+1;
            if (idProperty != null)
            {
                idProperty.SetValue(entity, nextIdValue, null);
            } else
            {
                var idField = typeof (T).GetField("Id", BindingFlags.Instance);
                if (idField != null)
                {
                    idField.SetValue(entity, nextIdValue);
                }
            }

            repo.Add(entity);
        }

        public void Commit()
        {
            // nothing to do
        }
    }
}