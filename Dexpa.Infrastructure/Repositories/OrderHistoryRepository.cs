using System.Data.Entity;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Infrastructure.Repositories
{
    public class OrderHistoryRepository:ARepository<OrderHistory>,IOrderHistoryRepository
    {
        public OrderHistoryRepository(DbContext context) : base(context)
        {
        }
    }
}
