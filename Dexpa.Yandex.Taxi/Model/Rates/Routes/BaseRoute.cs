using System.Collections.Generic;
using Yandex.Taxi.Model.Rates.Services;

namespace Yandex.Taxi.Model.Rates.Routes
{
    public abstract class BaseRoute
    {
        protected BaseRoute()
        {
            Services = new List<BaseService>();
        }

        public List<BaseService> Services { get; private set; }

        public LocalizedString Comment { get; set; }
    }
}