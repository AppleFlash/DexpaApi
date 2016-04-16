using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Infrastructure
{
    public class TransactionsFactory
    {
        private DbContext mDbContext;

        public TransactionsFactory(DbContext dbContext)
        {
            mDbContext = dbContext;
        }

        public IDbTransaction BeginTransaction()
        {
            return mDbContext.Database.BeginTransaction().UnderlyingTransaction;
        }
    }
}
