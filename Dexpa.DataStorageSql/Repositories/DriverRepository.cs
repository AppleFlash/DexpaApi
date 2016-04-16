using System.Data.Entity;
using Dexpa.Core;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.DataStorageSql.Repositories
{
    public class DriverRepository : ARepository<Driver>, IDriverRepository
    {
        public DriverRepository(DbContext context)
            : base(context)
        {
        }
    }
}
