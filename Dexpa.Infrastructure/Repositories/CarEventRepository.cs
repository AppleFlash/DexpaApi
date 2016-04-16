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
    public class CarEventRepository : ARepository<CarEvent>, ICarEventRepository
    {
        public CarEventRepository(DbContext context)
            : base(context)
        {
        }
    }
}
