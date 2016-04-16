using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Yandex.Taxi.Model.Tracks
{
    [YAXSerializeAs("tracks")]
    public class Tracks
    {
        /// <summary>
        /// Client ID
        /// </summary>
        [YAXSerializeAs("clid")]
        [YAXAttributeForClass]
        public string Clid { get; set; }

        [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement, EachElementName = "track")]
        public List<Track> Items { get; private set; }

        public Tracks()
        {
            this.Items = new List<Track>();
        }
    }
}
