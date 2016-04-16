using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Infrastructure.Repositories
{
    public class RegionRepository : ARepository<Region>, IRegionRepository
    {
        protected DbSet<RegionPoint> mRegionPointsSet;

        public RegionRepository(DbContext context)
            : base(context)
        {
            mRegionPointsSet = mContext.Set<RegionPoint>();
        }

        public IList<RegionPoint> GetAllPoints()
        {
            return mRegionPointsSet.ToList();
        }
    }
}
