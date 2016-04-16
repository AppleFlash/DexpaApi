using System;
using System.Collections.Generic;
using Dexpa.Core.Model;

namespace Dexpa.Core.Services
{
    public interface IRegionService : IDisposable
    {
        Region GetRegion(long regionId);

        IList<Region> GetRegions();

        IList<RegionPoint> GetAllPoints();
    }
}