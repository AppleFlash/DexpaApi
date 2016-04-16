using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Infrastructure.Repositories
{
    public class RepairRepository : ARepository<Repair>, IRepairRepository
    {
        public RepairRepository(DbContext context)
            : base(context)
        {
        }
    }
}
