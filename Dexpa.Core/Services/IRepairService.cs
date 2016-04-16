using System.Collections.Generic;
using Dexpa.Core.Model;

namespace Dexpa.Core.Services
{
    public interface IRepairService
    {
        Repair GetRepair(long repairId);

        Repair AddRepair(Repair repair);

        void DeleteRepair(long repairId);

        Repair UpdateRepair(Repair repair);

        IList<Repair> GetCarRepairs(long carId);
    }
}