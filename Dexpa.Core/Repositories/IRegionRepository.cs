using System.Collections.Generic;
using Dexpa.Core.Model;

namespace Dexpa.Core.Repositories
{
    public interface IRegionRepository : IRepository<Region>
    {
        IList<RegionPoint> GetAllPoints();
    }
}
