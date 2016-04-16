using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Core.Services
{
    public class WayBillsService:IWayBillsService
    {
        private IWayBillsRepository mRepository;
        private IDriverRepository mDriverRepository;
        private ICarRepository mCarRepository;

        public WayBillsService(IWayBillsRepository repository, IDriverRepository driverRepository, ICarRepository carRepository)
        {
            mRepository = repository;
            mDriverRepository = driverRepository;
            mCarRepository = carRepository;
        }

        public IList<WayBills> GetWayBillses(long? driverId = null, long? carId = null, DateTime? fromDate = null, DateTime? toDate = null, bool isActive = false)
        {
            IList<WayBills> wayBills;
            if (driverId == null && carId==null)
            {
                wayBills =  mRepository.List(w => w.FromDate >= fromDate && w.FromDate <= toDate);
            }
            else
            {
                if (driverId == null)
                {
                    wayBills = mRepository.List(w => w.CarId == carId && w.FromDate >= fromDate && w.FromDate <= toDate);
                }
                else if (carId == null)
                {
                    wayBills = mRepository.List(w => w.DriverId == driverId && w.FromDate >= fromDate && w.FromDate <= toDate);
                }
                else
                {
                    wayBills = mRepository.List(w => w.DriverId == driverId && w.CarId == carId && w.FromDate >= fromDate && w.FromDate <= toDate);
                }
            }
            if (isActive)
            {
                wayBills = wayBills.Where(w => w.EndMileage == 0).ToList();
            }
            return wayBills;
        }

        public WayBills GetWayBills(long id)
        {
            return mRepository.Single(w => w.Id == id);
        }

        public WayBills GetDriverWayBills(long driverId)
        {
            return mRepository.List(w=>w.DriverId==driverId).LastOrDefault();
        }

        public WayBills AddWayBills(WayBills wayBills)
        {
            var oldWayBills = GetDriverWayBills(wayBills.DriverId);
            if (oldWayBills != null)
            {
                if (oldWayBills.EndMileage == 0)
                {
                    oldWayBills.EndMileage = oldWayBills.StartMileage;
                    UpdateWayBills(oldWayBills);
                }
            }
            wayBills = mRepository.Add(wayBills);
            mRepository.Commit();
            return wayBills;
        }

        public WayBills UpdateWayBills(WayBills wayBills)
        {
            UpdateWayBillsRelations(wayBills);
            wayBills = mRepository.Update(wayBills);
            mRepository.Commit();
            return wayBills;
        }

        private void UpdateWayBillsRelations(WayBills wayBills)
        {
            Driver driver = null;
            Car car = null;

            if (wayBills.DriverId == 0)
            {
                wayBills.Driver = null;
            }
            if (wayBills.CarId == 0)
            {
                wayBills.Car = null;
            }

            if (wayBills.DriverId!=0)
            {
                driver = mDriverRepository.Single(d => d.Id == wayBills.DriverId);
            }
            if (wayBills.CarId != 0)
            {
                car = mCarRepository.Single(c => c.Id == wayBills.CarId);
            }

            wayBills.Driver = driver;
            wayBills.Car = car;
        }

        public void DeleteWayBills(long id)
        {
            var wayBills = mRepository.Single(w => w.Id == id);
            mRepository.Delete(wayBills);
            mRepository.Commit();
        }
    }
}
