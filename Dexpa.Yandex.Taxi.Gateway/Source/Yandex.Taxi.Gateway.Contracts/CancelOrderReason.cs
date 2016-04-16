using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yandex.Taxi.Gateway.Contracts
{
    public enum CancelOrderReason
    {
        /// <summary>
        /// Отозван клиентом
        /// </summary>
        User,
        /// <summary>
        /// Назначен на водителя другой Службы Такси
        /// </summary>
        Assigned,
        /// <summary>
        /// ни один водитель не сообщил о готовности выполнить заказ в течение отведенного времени
        /// </summary>
        Timeout
    }
}
