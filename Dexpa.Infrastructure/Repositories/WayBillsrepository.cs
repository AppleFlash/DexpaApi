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
    public class WayBillsrepository:ARepository<WayBills>,IWayBillsRepository
    {
        public WayBillsrepository(DbContext context) : base(context)
        {
        }
    }
}
