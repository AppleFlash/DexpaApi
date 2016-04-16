using System.Data.Entity;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Infrastructure.Repositories
{
    public class CarRepository : ARepository<Car>, ICarRepository
    {
        public CarRepository(DbContext context)
            : base(context)
        {
        }
    }
}
