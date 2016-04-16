using YAXLib;

namespace Yandex.Taxi.Model.Rates.Services
{
    public abstract class BaseService
    {
        protected BaseService(ServiceType eType)
        {
            Type = eType;
        }

        [YAXAttributeForClass]
        public ServiceType Type { get; private set; }
    }
}