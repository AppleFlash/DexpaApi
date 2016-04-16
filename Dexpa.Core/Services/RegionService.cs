using System.Collections.Generic;
using System.Linq;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Core.Services
{
    public class RegionService : IRegionService
    {
        private readonly IRegionRepository mRegionRepository;

        public RegionService(IRegionRepository regionRepository)
        {
            mRegionRepository = regionRepository;
        }

        public Region GetRegion(long regionId)
        {
            return mRegionRepository.Single(r => r.Id == regionId);
        }

        public IList<Region> GetRegions()
        {
            return mRegionRepository.List();
        }

        public IList<RegionPoint> GetAllPoints()
        {
            return mRegionRepository.GetAllPoints();
        }

        public void Dispose()
        {
            mRegionRepository.Dispose();
        }
    }
}
