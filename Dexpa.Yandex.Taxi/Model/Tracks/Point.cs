using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Yandex.Taxi.Model.Tracks
{
    public class Point
    {
        [YAXSerializeAs("latitude")]
        [YAXAttributeForClass]
        public decimal Latitude { get; set; }

        [YAXSerializeAs("longitude")]
        [YAXAttributeForClass]
        public decimal Longitude { get; set; }

        [YAXSerializeAs("avg_speed")]
        [YAXAttributeForClass]
        public int AverageSpeed { get; set; }

        [YAXSerializeAs("direction")]
        [YAXAttributeForClass]
        public int Direction { get; set; }

        /// <summary>
        /// Time UTC
        /// </summary>
        [YAXSerializeAs("time")]
        [YAXFormat("ddMMyyyy:HHmmss")]
        [YAXAttributeForClass]
        public DateTime Time { get; set; }
    }
}
