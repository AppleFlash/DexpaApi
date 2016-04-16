using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yandex.Taxi.Gateway.Contracts
{
    public interface IListener
    {
        /// <summary>
        /// Отмена заказа
        /// </summary>
        /// <remarks>Яндекс.Такси может отменить заказ только до подачи машины (в период между закреплением заказа
        /// за водителем и выставлением статуса заказа «waiting»). Если Яндекс.Такси присылает запрос об отмене
        /// заказа после подачи машины, Служба Такси должна вернуть HTTP-ответ с кодом 400
        /// Bad Request.</remarks>
        /// <returns></returns>
        CancelOrderResult CancelOrder(string sOrderId, CancelOrderReason eReason);

        /// <summary>
        /// Предложение заказа
        /// </summary>
        /// <param name="order"></param>
        void NewOrder(IOrder order);

        /// <summary>
        /// Яндекс.Такси присылает запрос, закрепляющий заказ за водителем Службы Такси. Чтобы подтвердить
        /// закрепление, Служба Такси должна ответить HTTP-кодом 200 (все варианты ответа описаны в разделе Ответ).
        /// </summary>
        void CommitOrder(IOrder order);

        /// <summary>
        /// Запрос стоимости заказа
        /// </summary>
        /// <param name="dtStartTripTimeUTC">Время начала поездки</param>
        /// <param name="tripRoute">Точки маршрута от места подачи машины до места назначения</param>
        void CalculateTripCost(DateTime dtStartTripTimeUTC, IRoute tripRoute);
    }
}
