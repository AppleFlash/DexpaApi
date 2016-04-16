using System.Collections.Generic;
using Dexpa.Core.Model;

namespace Dexpa.Core.Services
{
    public interface ICarEventService
    {
        CarEvent GetCarEvent(long carEventId);

        CarEvent AddCarEvent(CarEvent carEvent);

        void DeleteCarEvent(long carEventId);

        CarEvent UpdateCarEvent(CarEvent carEvent);

        IList<CarEvent> GetCarEventByCar(long carId);
    }
}