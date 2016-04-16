using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Yandex.Taxi.Model.Drivers
{
    public class Car
    {
        public string Uuid { get; set; }

        [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
        [YAXDontSerialize]
        public string RealClid { get; set; }

        [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
        [YAXDontSerialize]
        public string RealName { get; set; }

        [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
        [YAXDontSerialize]
        public string RealWeb { get; set; }

        [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
        [YAXDontSerialize]
        public string RealScid { get; set; }

        [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement, EachElementName = "Tariff")]
        public List<string> Tariffs { get; private set; }

        public DriverDetails DriverDetails { get; private set; }

        public CarDetails CarDetails { get; private set; }

        public Car()
        {
            this.Tariffs = new List<string>();
            this.CarDetails = new CarDetails();
            this.DriverDetails = new DriverDetails();
        }
    }
}
