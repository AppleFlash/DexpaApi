using System.Collections.Generic;
using YAXLib;

namespace Yandex.Taxi.Model.Rates
{
    public class Span
    {
        public Span()
        {
            Days = new List<Day>();
        }

        [YAXCollection(YAXCollectionSerializationTypes.Recursive, EachElementName = "Item")]
        public List<Day> Days { get; private set; }

        public TimeInterval TimeInterval { get; set; }
    }
}