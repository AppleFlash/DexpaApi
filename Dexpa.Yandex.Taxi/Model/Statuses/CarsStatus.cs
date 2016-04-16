using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yandex.Taxi.Model.Statuses
{
    public class CarsStatus
    {
        public List<Car> Cars { get; private set; }

        public CarsStatus()
        {
            this.Cars = new List<Car>();
        }
    }
}
