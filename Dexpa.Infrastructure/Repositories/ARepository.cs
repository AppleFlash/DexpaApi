using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Infrastructure.Repositories
{
    public abstract class ARepository<T> : IRepository<T>, IDisposable where T : class
    {
        protected DbContext mContext;

        protected DbSet<T> mSet;

        private const int MAX_TAKE = 1000;

        public ARepository(DbContext context)
        {
            mContext = context;
            mSet = mContext.Set<T>();
        }

        public int Count()
        {
            using (var transaction = mContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                return mSet.Count();
            }
        }

        public IList<T> List(bool withNoLock = true)
        {

            using (var transaction = mContext.Database.BeginTransaction(GetIsolationLevel(withNoLock)))
            {
                return mSet.ToList();
            }
        }

        private IsolationLevel GetIsolationLevel(bool withNoLock)
        {
            return withNoLock ?
                IsolationLevel.ReadUncommitted :
                IsolationLevel.Serializable;
        }

        public IList<T> List(int skip, int take, string sortBy, SortOrder orderBy)
        {
            var takeCount = take > MAX_TAKE ? MAX_TAKE : take;
            var sortExpression = string.Format("{0} {1}", sortBy, orderBy);
            return mSet
                .OrderBy(sortExpression)
                .Skip(skip)
                .Take(takeCount)
                .ToList();
        }

        public IList<T> List(Expression<Func<T, bool>> expression, int skip, int take, string sortBy, SortOrder? orderBy)
        {
            var takeCount = take > MAX_TAKE ? MAX_TAKE : take;
            if (!string.IsNullOrEmpty(sortBy) && orderBy.HasValue)
            {
                var sortExpression = string.Format("{0} {1}", sortBy, orderBy.Value);
                return mSet
                    .OrderBy(sortExpression)
                    .Skip(skip)
                    .Take(takeCount)
                    .ToList();
            }

            return mSet
                .Skip(skip)
                .Take(takeCount)
                .ToList();
        }

        public IList<T> List(Expression<Func<T, bool>> expression, bool withNoLock = true)
        {
            using (var transaction = mContext.Database.BeginTransaction(GetIsolationLevel(withNoLock)))
            {
                return mSet.Where(expression).ToList();
            }
        }

        public bool Any(Expression<Func<T, bool>> expression)
        {
            using (var transaction = mContext.Database.BeginTransaction(GetIsolationLevel(false)))
            {
                return mSet.Any(expression);
            }
        }

        public void ResetState(T entity)
        {
            mContext.Entry(entity).Reload();
        }

        public T Single(Expression<Func<T, bool>> expression)
        {
            using (var transaction = mContext.Database.BeginTransaction(GetIsolationLevel(false)))
            {
                //TODO: SingleOrDefault???
                return mSet.FirstOrDefault(expression);
            }
        }

        public T Add(T item)
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
            return item;
        }

        public T Update(T item)
        {
            mContext.Entry(item).State = EntityState.Modified;
            return item;
        }

        public virtual void Delete(T item)
        {
            mContext.Entry(item).State = EntityState.Deleted;
        }

        public bool IsItemPropertyChanged(T item, string propertyName)
        {
            if (item == null)
            {
                return false;
            }
            var dbEntityEntry = mContext.Entry(item);
            if (dbEntityEntry.State == EntityState.Modified)
            {
                var originalValue = dbEntityEntry.Property(propertyName).OriginalValue;
                var newValue = dbEntityEntry.Property(propertyName).CurrentValue;

                return originalValue == null && newValue != null ||
                    originalValue != null && !originalValue.Equals(newValue);
            }
            else if (dbEntityEntry.State == EntityState.Added)
            {
                return true;
            }
            return false;
        }

        public object GetOldValue(T item, string propertyName)
        {
            var dbEntityEntry = mContext.Entry(item);
            return dbEntityEntry.Property(propertyName).OriginalValue;
        }

        public object GetNewValue(T item, string propertyName)
        {
            var dbEntityEntry = mContext.Entry(item);
            return dbEntityEntry.Property(propertyName).CurrentValue;
        }

        public void Commit()
        {
            mContext.SaveChanges();
        }

        public IDbTransaction BeginTransaction()
        {
            return mContext.Database.BeginTransaction().UnderlyingTransaction;
        }

        public void Dispose()
        {
            mContext.Dispose();
        }
    }
}
