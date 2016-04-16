using System.Data.Entity;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Infrastructure.Repositories
{
    public class TariffRepository : ARepository<Tariff>, ITariffRepository
    {
        public TariffRepository(DbContext context)
            : base(context)
        {
        }
    }
}
