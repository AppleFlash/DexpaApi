using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yandex.Taxi.Gateway.Contracts
{
    public enum ReadyToProceedOrderResult
    {
        Ok,
        DriverOrOrderNotFound,
        /// <summary>
        /// Заказ закреплен за другим водителем
        /// </summary>
        /// <remarks>Получив такой код ответа, Служба Такси должна отменить заказ, не дожидаясь отдельной команды от сервера Яндекс.Такси</remarks>
        Assigned,
        Unknown
    }
}
