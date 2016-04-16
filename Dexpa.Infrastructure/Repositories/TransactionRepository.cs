using System.Data.Entity;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Infrastructure.Repositories
{
    public class TransactionRepository : ARepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(DbContext context)
            : base(context)
        {
        }
    }
}
