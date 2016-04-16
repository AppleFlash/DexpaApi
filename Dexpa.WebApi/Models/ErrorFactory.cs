using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dexpa.WebApi.Models
{
    static class ErrorFactory
    {
        public static Error DriverNotFound()
        {
            return new Error
            {
                ErrorCode = 1,
                LocalizedMessage = "Доступ запрещен, обратитесь в диспетчерскую"
            };
        }

        public static Error NotEnoughtMoney()
        {
            return new Error
            {
                ErrorCode = 2,
                LocalizedMessage = "Недостаточно средств, пополните счет. Либо вы не допущены к работе"
            };
        }

        public static Error OrderNotFound(long id)
        {
            return new Error
            {
                ErrorCode = 3,
                LocalizedMessage = "Заказа с номером " + id + " не существует"
            };
        }

        public static Error OrderGivenAnotherDriver()
        {
            return new Error
            {
                ErrorCode = 4,
                LocalizedMessage = "Заказ отдан другому водителю"
            };
        }

        public static Error YouCantWork()
        {
            return new Error
            {
                ErrorCode = 5,
                LocalizedMessage = "Вы не допущены к работе"
            };
        }

        public static Error OrderExecutionInProcess()
        {
            return new Error
            {
                ErrorCode = 6,
                LocalizedMessage = "Вы не можете изменить статус, пока не завершите текущий заказ"
            };
        }
    }
}