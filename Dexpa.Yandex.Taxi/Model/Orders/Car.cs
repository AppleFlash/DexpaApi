using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace Yandex.Taxi.Model.Orders
{
    /// <summary>
    /// Информация об одном из подобранных водителей.
    /// </summary>
    public class Car
    {
        /// <summary>
        /// Уникальный идентификатор водителя в рамках Службы Такси.
        /// </summary>
        public string Uuid { get; set; }

        /// <summary>
        /// Расстояние от водителя до клиента по кратчайшему маршруту в километрах.
        /// </summary>
        [YAXSerializeAs("Dist")]
        [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
        public decimal Distance { get; set; }

        /// <summary>
        /// Время следования по кратчайшему маршруту с учетом пробок, в минутах (целое число).
        /// </summary>
        [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
        public decimal Time { get; set; }

        /// <summary>
        /// Тариф, по которому данному водителю предлагается принять запрос клиента.
        /// </summary>
        public string Tariff { get; set; }

        /// <summary>
        /// Ссылка на статическую карту, на которой отмечено положение водителя и место подачи ма-
        /// шины. С помощью карты водитель может быстро оценить расстояние до места подачи и ре-
        /// шить, стоит ли принимать заказ.
        /// </summary>
        [YAXErrorIfMissed(YAXExceptionTypes.Ignore)]
        public string MapHref { get; set; }

    }
}
