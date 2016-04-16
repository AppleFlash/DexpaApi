using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dexpa.Core.Model;

namespace Dexpa.Core.Repositories
{
    public interface IRepository<T> : IDisposable
    {
        IList<T> List(bool withNoLock = true);

        IList<T> List(int skip, int take, string sortBy, SortOrder orderBy);

        IList<T> List(Expression<Func<T, bool>> expression, int skip, int take, string sortBy = null, SortOrder? orderBy = null);

        IList<T> List(Expression<Func<T, bool>> expression, bool withNoLock = true);

        bool Any(Expression<Func<T, bool>> expression);

        T Single(Expression<Func<T, bool>> expression);

        int Count();

        bool IsItemPropertyChanged(T item, string propertyName);

        object GetOldValue(T item, string propertyName);

        object GetNewValue(T item, string propertyName);

        void ResetState(T entity);

        void Commit();
    }
}
