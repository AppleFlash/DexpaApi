using System.Collections.Generic;
using YAXLib;

namespace Yandex.Taxi.Model.Rates
{
    public class Schedule
    {
        public Schedule()
        {
            Spans = new List<Span>();
        }

        [YAXCollection(YAXCollectionSerializationTypes.Recursive, EachElementName = "Span")]
        public List<Span> Spans { get; private set; }
    }
}