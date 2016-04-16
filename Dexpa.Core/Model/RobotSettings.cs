using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.Core.Model
{
    [ComplexType]
    public class RobotSettings
    {
        public bool Enabled { get; set; }

        public int OrderRadius { get; set; }

        public bool Airports { get; set; }

        public bool OrdersSequence { get; set; }

        public bool WantToHome { get; set; }

        public string AddressSearch { get; set; }

        public int MinutesDepartureTime { get; set; }

        public RobotSettings()
        {

        }
    }
}
