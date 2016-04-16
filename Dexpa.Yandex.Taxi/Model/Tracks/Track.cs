using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Yandex.Taxi.Model.Tracks
{
    public class Track
    {
        [YAXSerializeAs("uuid")]
        [YAXAttributeForClass]
        public string Uuid { get; set; }

        [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement, EachElementName = "point")]
        public List<Point> Points { get; private set; }

        public Track()
        {
            this.Points = new List<Point>();
        }
    }
}
