using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using YAXLib;

namespace Yandex.Taxi.Model.Rates.Routes
{
    [YAXCustomSerializer(typeof (RouteSerializer))]
    public class Route
    {
        public Area Departure { get; set; }

        public Area Arrival { get; set; }

        public decimal MinPrice { get; set; }
    }

    public class RouteSerializer : ICustomSerializer<Route>
    {
        public void SerializeToAttribute(Route objectToSerialize, XAttribute attrToFill)
        {
            throw new NotImplementedException();
        }

        public void SerializeToElement(Route objectToSerialize, XElement elemToFill)
        {
            elemToFill.Add(
                new XElement(XName.Get("Area"), new XAttribute(XName.Get("Type"), "source"),
                    objectToSerialize.Departure.ToString().ToLowerInvariant()),
                new XElement(XName.Get("Area"), new XAttribute(XName.Get("Type"), "destination"),
                    objectToSerialize.Arrival.ToString().ToLowerInvariant()),
                new XElement(XName.Get("MinPrice"), objectToSerialize.MinPrice.ToString(CultureInfo.InvariantCulture))
                );
        }

        public string SerializeToValue(Route objectToSerialize)
        {
            throw new NotImplementedException();
        }

        public Route DeserializeFromAttribute(XAttribute attrib)
        {
            throw new NotImplementedException();
        }

        public Route DeserializeFromElement(XElement element)
        {
            var result = new Route();

            string sDeparture = element.XPathSelectElement("./Area[@Type = 'source']").Value;
            string sArrival = element.XPathSelectElement("./Area[@Type = 'destination']").Value;
            string sMinPrice = element.Elements("MinPrice").Single().Value;

            result.Departure = ParseArea(sDeparture);
            result.Arrival = ParseArea(sArrival);
            result.MinPrice = int.Parse(sMinPrice);

            return result;
        }

        public Route DeserializeFromValue(string value)
        {
            throw new NotImplementedException();
        }

        private static Area ParseArea(string sValue)
        {
            return (Area) Enum.Parse(typeof (Area), sValue, true);
        }
    }
}