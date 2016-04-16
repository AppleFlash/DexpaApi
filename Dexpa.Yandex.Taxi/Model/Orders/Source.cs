using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Yandex.Taxi.Model.Orders
{
    public class Source
    {
        /// <summary>
        /// Полный адрес места подачи на русском языке, от страны до номера дома. Номер подъезда
        /// включается в адрес, если клиент указал подъезд при заказе такси.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Короткий адрес места подачи машины (улица и номер дома).
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Станция метро, ближайшая к месту подачи машины. Элемент отсутствует, если ближайшую
        /// станцию метро не удалось определить.
        /// </summary>
        [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
        public string ClosestStation { get; set; }

        /// <summary>
        /// Адрес места подачи, разбитый на элементы. Соответствует адресу, указанному в элементе
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        /// Информация о встрече клиента в аэропорте. Необязательный элемент.
        /// </summary>
        [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
        public Airport Airport { get; set; }

        /// <summary>
        /// Географические координаты места подачи машины. Необязательный элемент.
        /// </summary>
        [YAXErrorIfMissed(YAXLib.YAXExceptionTypes.Ignore)]
        public Point Point { get; set; }
    }
}
