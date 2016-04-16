using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YAXLib;

namespace Yandex.Taxi.Model.Orders
{
    public class ContactInfo
    {
        /// <summary>
        /// Список контактных телефонов клиента.
        /// </summary>
        [YAXCollection(YAXCollectionSerializationTypes.Recursive, EachElementName="Phone")]
        public List<string> Phones { get; set; }

        /// <summary>
        /// Имя в том виде, в котором его предоставил клиент. Необязательный элемент.
        /// </summary>
        [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
        public string Name { get; set; }
    }
}
