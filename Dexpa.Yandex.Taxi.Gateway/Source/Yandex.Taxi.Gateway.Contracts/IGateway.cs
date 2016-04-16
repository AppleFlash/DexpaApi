using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yandex.Taxi.Gateway.Contracts
{
    public interface IGateway
    {
        /// <summary>
        /// Отослать статусы водителей
        /// </summary>
        /// <param name="statuses"></param>
        void SendDriversStatus(params DriverStatus[] statuses);

        /// <summary>
        /// Отослать сообщение о готовности выполнить заказ
        /// </summary>
        /// <param name="sUuid">Id водителя</param>
        /// <param name="sOrderId">Id подтверждаемого заказа</param>
        /// <param name="iCost">Цена поездки по маршруту, указанному в заказе</param>
        ReadyToProceedOrderResult SendReadyToProceedOrder(string sUuid, string sOrderId, int? iCost = null);

        /// <summary>
        /// Обновление статуса заказа
        /// </summary>
        /// <param name="sOrderId">Идентификатор заказа, статус которого обновляется</param>
        /// <param name="eStatus">Новый статус заказа</param>
        /// <param name="sExtra">Дополнительная информация, значение которой интерпретируется в зависимости
        /// от указанного статуса заказа, передаваемого с помощью параметра eStatus:
        /// <list type="bullet">
        /// <item>
        /// <description>«complete» — в параметре следует указать общую цену поездки</description>
        /// </item>
        /// <item>
        /// <description>«failed» и «cancelled» — в параметре следует указать причину невыполнения заказа</description>
        /// </item>
        /// <item>
        /// <description>«driving» — в параметре следует указать идентификатор водителя, выполняющего заказ</description>
        /// </item>
        /// </list>
        /// В остальных случаях параметр extra игнорируется.</param>
        /// <param name="sNewCar">Идентификатор нового водителя, назначенного на заказ. Указывается, если по каким-то
        /// причинам Служба Такси заменяет водителя, который был закреплен за заказом ранее.
        /// Новый водитель должен присутствовать в базе Яндекс.Такси
        /// Назначая нового водителя, статус заказа можно выставить любым — при наличии
        /// параметра newcar хронологические ограничения не накладываются.
        /// </param>
        OrderUpdateResult SendOrderUpdate(string sOrderId, OrderStatus eStatus, string sExtra = null, string sNewCar = null);
    }
}
