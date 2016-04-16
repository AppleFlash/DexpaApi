using System.Collections.Generic;
using System.Linq;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Core.Services
{
    public class RepairService : IRepairService
    {
        private readonly IRepairRepository mRepairRepository;
        private readonly ICarRepository mCarRepository;
        private readonly IDriverRepository mDriverRepository;

        public RepairService(IRepairRepository repairRepository,ICarRepository carRepository, IDriverRepository driverRepository)
        {
            mRepairRepository = repairRepository;
            mCarRepository = carRepository;
            mDriverRepository = driverRepository;
        }

        public Repair GetRepair(long repairId)
        {
            return mRepairRepository.Single(c => c.Id == repairId);
        }

        public Repair AddRepair(Repair repair)
        {

            repair = mRepairRepository.Add(repair); ;
            mRepairRepository.Commit();
            return repair;
        }

        public void DeleteRepair(long repairId)
        {
            var repair = mRepairRepository.Single(c => c.Id == repairId);
            if (repair != null)
            {
                mRepairRepository.Delete(repair);
                mRepairRepository.Commit();
            }
        }

        public Repair UpdateRepair(Repair repair)
        {
            var origRepair = mRepairRepository.Single(r => r.Id == repair.Id);

            if (origRepair == null)
            {
                return repair;
            }

            origRepair.GuiltyDriverId = repair.GuiltyDriverId;
            origRepair.ImplementedById = repair.ImplementedById;
            origRepair.CarId = repair.CarId;
            origRepair.Comment = repair.Comment;
            origRepair.Cost = repair.Cost;
            origRepair.DamagesPhotos = repair.DamagesPhotos;

            repair = mRepairRepository.Update(origRepair);
            mRepairRepository.Commit();
            return repair;
        }

        public IList<Repair> GetCarRepairs(long carId)
        {
            return mRepairRepository.List(r => r.CarId == carId).OrderBy(r=>r.Timestamp).ToList();
        }
    }
}
