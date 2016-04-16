using System.Collections.Generic;

namespace Yandex.Taxi.Model.Rates.Routes
{
    public class FixedRoute : BaseRoute
    {
        public FixedRoute()
        {
            Routes = new List<Route>();
        }

        public List<Route> Routes { get; private set; }
    }
}