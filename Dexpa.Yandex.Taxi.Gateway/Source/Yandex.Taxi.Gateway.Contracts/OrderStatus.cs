using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yandex.Taxi.Gateway.Contracts
{
    /// <summary>
    /// Перечисление возможнных статусов заказа
    /// <remarks>
    /// Статус заказа может изменяться только в указанном порядке. То есть, для заказа,
    /// находящегося в статусе «transporting», нельзя передать статус «waiting».
    /// Исключениями из правила являются статусы «cancelled» и «failed», которые могут
    /// быть переданы в любой момент.</remarks>
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// Водитель выехал к клиенту
        /// </summary>
        Driving,
        /// <summary>
        /// Водитель прибыл на место назначения, ожидает клиента
        /// </summary>
        Waiting,
        /// <summary>
        /// Водитель везет клиента к месту назначения
        /// </summary>
        Transporting,
        /// <summary>
        /// Заказ выполнен
        /// </summary>
        Complete,
        /// <summary>
        /// Заказ отменен клиентом
        /// </summary>
        Cancelled,
        /// <summary>
        /// Водитель не смог выполнить заказ
        /// </summary>
        Failed
    }
}
