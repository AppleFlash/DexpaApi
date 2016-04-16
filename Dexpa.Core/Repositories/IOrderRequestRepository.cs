using Dexpa.Core.Model;

namespace Dexpa.Core.Repositories
{
    public interface IOrderRequestRepository : ICRepository<OrderRequest>
    {
        void Delete(OrderRequest item);
    }
}
