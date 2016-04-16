using System.Collections.Generic;
using System.Linq;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Core.Services
{
    public class CarEventService : ICarEventService
    {
        private readonly ICarEventRepository mCarEventRepository;

        public CarEventService(ICarEventRepository carEventRepository)
        {
            mCarEventRepository = carEventRepository;
        }

        public CarEvent GetCarEvent(long carEventId)
        {
            return mCarEventRepository.Single(c => c.Id == carEventId);
        }

        public CarEvent AddCarEvent(CarEvent carEvent)
        {
            carEvent = mCarEventRepository.Add(carEvent); ;
            mCarEventRepository.Commit();
            return carEvent;
        }

        public void DeleteCarEvent(long carEventId)
        {
            var repair = mCarEventRepository.Single(c => c.Id == carEventId);
            if (repair != null)
            {
                mCarEventRepository.Delete(repair);
                mCarEventRepository.Commit();
            }
        }

        public CarEvent UpdateCarEvent(CarEvent carEvent)
        {
            carEvent = mCarEventRepository.Update(carEvent);
            mCarEventRepository.Commit();
            return carEvent;
        }

        public IList<CarEvent> GetCarEventByCar(long carId)
        {
            return mCarEventRepository.List(r => r.CarId == carId);
        }
    }
}
