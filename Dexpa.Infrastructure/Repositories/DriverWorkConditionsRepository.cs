using System.Data.Entity;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Infrastructure.Repositories
{
    public class DriverWorkConditionsRepository : ARepository<DriverWorkConditions>, IDriverWorkConditionsRepository
    {
        public DriverWorkConditionsRepository(DbContext context)
            : base(context)
        {
        }
    }
}
