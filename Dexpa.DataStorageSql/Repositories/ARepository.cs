using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Dexpa.Core.Repositories;

namespace Dexpa.DataStorageSql.Repositories
{
    public abstract class ARepository<T> : IRepository<T>, IDisposable where T : class
    {
        protected DbContext mContext;

        protected DbSet<T> mSet;

        public ARepository(DbContext context)
        {
            mContext = context;
            mSet = mContext.Set<T>();
        }

        public IList<T> List()
        {
            return mSet.ToList();
        }

        public IList<T> List(Expression<Func<T, bool>> expression)
        {
            return mSet.Where(expression).ToList();
        }

        public bool Any(Expression<Func<T, bool>> expression)
        {
            return mSet.Any(expression);
        }

        public void Add(T item)
        {
            var entiry = mContext.Entry(item);
            if (entiry.State != EntityState.Detached)
            {
                entiry.State = EntityState.Added;
            }
            else
            {
                mSet.Add(item);
            }
        }

        public void Update(T item)
        {
            mContext.Entry(item).State = EntityState.Modified;
        }

        public void Delete(T item)
        {
            mContext.Entry(item).State = EntityState.Deleted;
        }

        public void Dispose()
        {
            mContext.Dispose();
        }
    }
}
