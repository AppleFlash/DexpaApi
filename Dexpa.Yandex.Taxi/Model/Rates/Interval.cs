using System.Collections.Generic;
using Yandex.Taxi.Model.Rates.Routes;

namespace Yandex.Taxi.Model.Rates
{
    public class Interval
    {
        public Schedule Schedule { get; set; }

        public FreeRoute FreeRoute { get; set; }

        public List<FixedRoute> FixedRoutes { get; set; }
    }
}