using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dexpa.Core.Repositories;
using Dexpa.Core.Utils;
using Dexpa.Ioc;

namespace Dexpa.RegionPointsTool
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "RegionPoints";
            var regionPointsRepository = IocFactory.Instance.Create<IRegionRepository>();
            var points = regionPointsRepository.GetAllPoints();
            var storage = new RegionBinaryStorage(fileName);
            storage.SetPoints(points);
        }
    }
}
