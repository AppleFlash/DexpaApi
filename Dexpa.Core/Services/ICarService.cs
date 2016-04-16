using System;
using System.Collections.Generic;
using Dexpa.Core.Model;

namespace Dexpa.Core.Services
{
    public interface ICarService : IDisposable
    {
        Car GetCar(long carId);

        Car AddCar(Car car);

        void DeleteCar(long carId);

        Car UpdateCar(Car car);

        List<Car> GetCars(bool unassgined, long? includeDriverCar);

        List<string> GetCarModels();

        List<string> GetCarBrands();
    }
}