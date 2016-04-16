using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Yandex.Taxi.Model.Orders
{
    [YAXSerializeAs("Request")]
    public class OrderConfirmation
    {
        /// <summary>
        /// Уникальный идентификатор заказа, формируется сервисом Яндекс.Такси.
        /// </summary>
        [YAXSerializeAs("Orderid")]
        public string Id { get; set; }

        /// <summary>
        /// Список водителей, которых сервис Яндекс.Такси подобрал для срочного заказа. Необязательный элемент.
        /// Элемент не задается для несрочных заказов: Служба Такси должна подобрать водителя самостоятельно.
        /// </summary>
        [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
        public List<Car> Cars { get; set; }

        /// <summary>
        /// Контактная информация клиента.
        /// </summary>
        public ContactInfo ContactInfo { get; set; }

        /// <summary>
        /// Адреса всех промежуточных остановок и пункта назначения. Необязательный элемент
        /// </summary>
        public List<Destination> Destinations { get; set; }

        /// <summary>
        /// Описание места подачи машины.
        /// </summary>
        public Source Source { get; set; }

        /// <summary>
        /// Время подачи такси.
        /// </summary>
        public BookingTime BookingTime { get; set; }

        /// <summary>
        /// Дополнительные требования к машине. Необязательный элемент.
        /// </summary>
        public List<Requirement> Requirements { get; set; }

        [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
        public string Comments { get; set; }

        public OrderConfirmation()
        {
            this.Cars = new List<Car>();
            this.Requirements = new List<Requirement>();
        }
    }
}
