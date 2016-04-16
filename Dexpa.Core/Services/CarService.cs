using System.Collections.Generic;
using System.Linq;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Core.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository mCarRepository;
        private readonly IDriverRepository mDriverRepository;

        public CarService(ICarRepository carRepository, IDriverRepository driverRepository)
        {
            mCarRepository = carRepository;
            mDriverRepository = driverRepository;
        }

        public Car GetCar(long carId)
        {
            return mCarRepository.Single(c => c.Id == carId);
        }

        public Car AddCar(Car car)
        {
            var existsCar = mCarRepository.Single(c => c.Callsign == car.Callsign || c.RegNumber == car.RegNumber);
            if (existsCar != null)
            {
                return null;
            }
            else
            {
                mCarRepository.Add(car);
                mCarRepository.Commit();
            }
            return car;
        }

        public void DeleteCar(long carId)
        {
            var car = mCarRepository.Single(c => c.Id == carId);
            if (car != null)
            {
                mCarRepository.Delete(car);
                mCarRepository.Commit();
            }
        }

        public Car UpdateCar(Car car)
        {
            var existsCar = mCarRepository.Single(c => c.Callsign == car.Callsign||c.RegNumber==car.RegNumber);
            if (existsCar != null && existsCar.Id != car.Id)
            {
                return null;
            }
            else
            {
                mCarRepository.Update(car);
                mCarRepository.Commit();
            }
            return car;
        }

        public List<Car> GetCars(bool unassigned, long? includeDriverCar)
        {
            var cars = mCarRepository.List();
            var drivers = mDriverRepository.List();
            if (unassigned)
            {
                var carIds = drivers
                    .Where(d => d.CarId != null && d.Id != includeDriverCar)
                    .Select(d => d.CarId)
                    .Distinct()
                    .ToList();
                return cars.Where(c => !carIds.Contains(c.Id)).ToList();
            }
            else
            {
                return cars.ToList();
            }
        }

        public List<string> GetCarModels()
        {
            var cars = mCarRepository.List();
            var carModels = cars.Select(c => c.Model).ToList();
            return carModels;
        }

        public List<string> GetCarBrands()
        {
            var cars = mCarRepository.List();
            var carBrands = cars.Select(c => c.Brand).ToList();
            return carBrands;
        }

        public void Dispose()
        {
            mCarRepository.Dispose();
            mDriverRepository.Dispose();
        }
    }
}
