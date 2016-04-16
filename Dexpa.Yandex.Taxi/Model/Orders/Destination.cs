using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Yandex.Taxi.Model.Orders
{
    public class Destination
    {
        /// <summary>
        /// Порядковый номер остановки в маршруте.
        /// </summary>
        [YAXAttributeForClass]
        [YAXSerializeAs("order")]
        public int Order { get; set; }

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
        /// Адрес места назначения, разбитый на элементы. Соответствует адресу, указанному в элементе <FullName>.
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        /// Географические координаты места назначения.
        /// </summary>
        public Point Point { get; set; }
    }
}
