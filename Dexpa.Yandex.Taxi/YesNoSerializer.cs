using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using YAXLib;

namespace Yandex.Taxi
{
    public class YesNoSerializer : ICustomSerializer<bool>
    {
        public bool DeserializeFromAttribute(XAttribute attrib)
        {
            return ConvertToBool(attrib.Value);
        }

        public bool DeserializeFromElement(XElement element)
        {
            return ConvertToBool(element.Value);
        }

        public bool DeserializeFromValue(string value)
        {
            return ConvertToBool(value);
        }

        public void SerializeToAttribute(bool objectToSerialize, XAttribute attrToFill)
        {
            attrToFill.Value = ConvertToString(objectToSerialize);
        }

        public void SerializeToElement(bool objectToSerialize, XElement elemToFill)
        {
            elemToFill.Value = ConvertToString(objectToSerialize);
        }

        public string SerializeToValue(bool objectToSerialize)
        {
            return ConvertToString(objectToSerialize);
        }

        private static bool ConvertToBool(string sValue)
        {
            if (string.Compare(sValue, "yes", true, CultureInfo.InvariantCulture) == 0)
            {
                return true;
            }
            else if (string.Compare(sValue, "no", true, CultureInfo.InvariantCulture) == 0)
            {
                return false;
            }
            else
            {
                throw new NotSupportedException(string.Format("Cannot convert [{0}] to bool", sValue));
            }
        }

        private static string ConvertToString(bool bValue)
        {
            if (bValue)
            {
                return "yes";
            }
            else
            {
                return "no";
            }
        }
    }
}
