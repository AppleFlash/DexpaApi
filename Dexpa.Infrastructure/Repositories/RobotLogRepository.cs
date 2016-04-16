using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;
using EntityFramework.BulkInsert.Extensions;

namespace Dexpa.Infrastructure.Repositories
{
    public class RobotLogRepository : ARepository<RobotLog>, IRobotLogRepository
    {
        public RobotLogRepository(DbContext context)
            : base(context)
        {
        }

        public void AddLogs(IList<RobotLog> logs)
        {
            mContext.BulkInsert(logs);
            mContext.SaveChanges();
        }
    }
}
