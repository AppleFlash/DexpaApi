using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Yandex.Taxi.Model.Drivers
{
    public class CarDetails
    {
        public string Model { get; set; }

        public int Age { get; set; }

        public string Color { get; set; }

        public string CarNumber { get; set; }

        [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement, EachElementName = "Require")]
        public List<Requirement> Requirements { get; private set; }

        public CarDetails()
        {
            this.Requirements = new List<Requirement>();
        }
    }
}
