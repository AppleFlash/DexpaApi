using System;
using System.Globalization;
using System.Xml.Linq;
using YAXLib;

namespace Yandex.Taxi.Model.Rates
{
    public class TimeInterval
    {
        [YAXCustomSerializer(typeof (TimeSpanSerializer))]
        public TimeSpan Start { get; set; }

        [YAXCustomSerializer(typeof (TimeSpanSerializer))]
        public TimeSpan End { get; set; }
    }

    public class TimeSpanSerializer : ICustomSerializer<TimeSpan>
    {
        public void SerializeToAttribute(TimeSpan objectToSerialize, XAttribute attrToFill)
        {
            attrToFill.Value = objectToSerialize.ToString(@"hh\:mm", CultureInfo.InvariantCulture);
        }

        public void SerializeToElement(TimeSpan objectToSerialize, XElement elemToFill)
        {
            elemToFill.Value = objectToSerialize.ToString(@"hh\:mm", CultureInfo.InvariantCulture);
        }

        public string SerializeToValue(TimeSpan objectToSerialize)
        {
            return objectToSerialize.ToString(@"hh\:mm", CultureInfo.InvariantCulture);
        }

        public TimeSpan DeserializeFromAttribute(XAttribute attrib)
        {
            return TimeSpan.ParseExact(attrib.Value, @"hh\:mm", CultureInfo.InvariantCulture);
        }

        public TimeSpan DeserializeFromElement(XElement element)
        {
            return TimeSpan.ParseExact(element.Value, @"hh\:mm", CultureInfo.InvariantCulture);
        }

        public TimeSpan DeserializeFromValue(string value)
        {
            return TimeSpan.ParseExact(value, @"hh\:mm", CultureInfo.InvariantCulture);
        }
    }
}