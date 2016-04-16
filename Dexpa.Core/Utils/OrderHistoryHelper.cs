using System;
using System.Collections.Generic;
using System.Linq;
using Dexpa.Core.Model;
using Dexpa.Core.Services;

namespace Dexpa.Core.Utils
{
    static class OrderHistoryHelper
    {
        public static string GetStateMessage(OrderStateType orderState, long orderId, OrderSource orderSource, Driver driver)
        {
            string message = "";
            switch (orderState)
            {
                case OrderStateType.Created:
                    message = "Заказ #" + orderId + " создан через " + GetOrderSourceName(orderSource);
                    break;
                case OrderStateType.Assigned:
                    message = "Водитель [" + driver.Car.Callsign + "] " + driver.LastName + " " + driver.FirstName +
                              " " +
                              (!string.IsNullOrWhiteSpace(driver.MiddleName) ? driver.MiddleName : "") +
                              " назначен на заказ";
                    break;
                case OrderStateType.Accepted:
                    message = "Водитель принял заказ";
                    break;
                case OrderStateType.Approved:
                    message = GetOrderSourceName(orderSource) + " подтвердил закрепление заказа";
                    break;
                //case OrderStateType.Disapproved:
                //    message = GetOrderSourceName(order.Source) + " не подтвердил закрепление заказа";
                //    break;
                case OrderStateType.Driving:
                    message = "Водитель выехал на заказ";
                    break;
                case OrderStateType.Waiting:
                    message = "Водитель прибыл на место и ожидает клиента";
                    break;
                case OrderStateType.Transporting:
                    message = "Водитель везет клиента";
                    break;
                case OrderStateType.Completed:
                    message = "Водитель успешно завершил заказ";
                    break;
                case OrderStateType.Failed:
                    message = "Заказ провален";
                    break;
                case OrderStateType.Canceled:
                    message = "Заказ отменен клиентом";
                    break;
                default:
                    message = "";
                    break;
            }
            return message;
        }

        public static string GetDriverMessage(Driver driver)
        {
            var message = "";
            if (driver != null)
            {
                message = "На заказ назначен новый водитель - [" + driver.Car.Callsign + "] " + driver.LastName + " " + driver.FirstName + " " +
                                 (!string.IsNullOrWhiteSpace(driver.MiddleName) ? driver.MiddleName : "");
            }
            else
            {
                message = "С заказа сняли водителя";
            }
            return message;
        }

        public static string GetAddressMessage(bool isFromAddress, string oldAddress)
        {
            var message = "";
            if (isFromAddress)
            {
                message = "Адрес подачи изменен. Старый адрес подачи: ";
            }
            else
            {
                message = "Адрес назначения изменен. Старый адрес назначения: ";
            }
            return message + oldAddress;
        }

        public static string GetDepartureTimeMessage(DateTime oldDepartureTime, DateTime departureTime)
        {
            var message = "Время подачи изменено c " + oldDepartureTime.ToString("dd.MM.yyyy HH:mm") + " на " + departureTime.ToString("dd.MM.yyyy HH:mm");
            return message;
        }

        public static string GetCommentMessage(string oldComment, string newComment)
        {
            var message = "Примечание изменено c " + oldComment + " на " + newComment;
            return message;
        }

        public static string GetSmsMessage(int smsType)
        {
            var message = "";
            switch (smsType)
            {
                case 0:
                    message = "Смс о назначении водителя доставлена клиенту";break;
                case 1:
                    message = "Смс о подаче доставлена клиенту"; break;
                case 2:
                    message = "Смс о замене водителя доставлена клиенту"; break;
                default:
                    message = ""; break;
            }
            return message;
        }

        public static string GetSourceMessage(OrderSource newSource, OrderSource oldSource)
        {
            var message = "Источник заказа изменен с " + GetOrderSourceName(oldSource) + " на " +
                          GetOrderSourceName(newSource);
            return message;
        }

        public static string GetTariffMessage(Tariff newTariff, Tariff oldTariff)
        {
            var message = "Тариф изменен с " + oldTariff.Name + " на " + newTariff.Name;
            return message;
        }

        public static string GetCategoriesMessage(CarFeatures newFeatures, CarFeatures oldFeatures)
        {
            var message = "Набор категорий изменен " +
                          (oldFeatures != CarFeatures.None ? "с " + GetOrderCategories(oldFeatures) : "") + " на " +
                          (newFeatures != CarFeatures.None
                              ? GetOrderCategories(newFeatures)
                              : "Отказ от категорий");
            return message;
        }

        public static string GetServicesMessage(CarFeatures newFeatures, CarFeatures oldFeatures)
        {
            var message = "Набор услуг изменен " +
                          (oldFeatures != CarFeatures.None ? "с " + GetOrderServices(oldFeatures) : "") + " на " +
                          (newFeatures != CarFeatures.None ? GetOrderServices(newFeatures) : "Отказ от услуг");
                          
            return message;
        }

        public static string GetChildrenSeatMessage(ChildrenSeat newFeatures, ChildrenSeat oldFeatures)
        {
            var message = "Детское кресло изменено с " + GetChildrenSeatName(oldFeatures) + " на " +
                          GetChildrenSeatName(newFeatures);
            return message;
        }

        public static string GetOrderdiscountMessage(byte newValue, byte oldValue)
        {
            var message = "Скидка изменена с " + oldValue + " на " + newValue;
            return message;
        }

        public static string GetCustomerMessage(Customer oldCustomer, Customer newCustomer)
        {
            var message = "Данные клиента изменены c " + oldCustomer.Name + " " + oldCustomer.Phone + " на " +
                          newCustomer.Name + " " + newCustomer.Phone;
            return message;
        }

        private static string GetOrderStateTypeName(OrderStateType type)
        {
            string name;
            switch (type)
            {
                case OrderStateType.Created:
                    name = "Новый";
                    break;
                case OrderStateType.Assigned:
                    name = "Назначен";
                    break;
                case OrderStateType.Driving:
                    name = "Выехал";
                    break;
                case OrderStateType.Waiting:
                    name = "На месте";
                    break;
                case OrderStateType.Transporting:
                    name = "В пути";
                    break;
                case OrderStateType.Completed:
                    name = "Завершен";
                    break;
                case OrderStateType.Rejected:
                    name = "Отменен";
                    break;
                case OrderStateType.Failed:
                    name = "Водитель не смог выполнить заказ";
                    break;
                case OrderStateType.Canceled:
                    name = "Отменен клиентом";
                    break;
                case OrderStateType.Accepted:
                    name = "Принят";
                    break;
                case OrderStateType.Approved:
                    name = "Подтвержден";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
            return name;
        }

        private static string GetOrderSourceName(OrderSource source)
        {
            string name;
            switch (source)
            {
                case OrderSource.Dispatcher:
                    name = "Диспетчерскую";
                    break;
                case OrderSource.Yandex:
                    name = "Яндекс";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("source");
            }
            return name;
        }

        private static string GetOrderCategories(CarFeatures features)
        {
            var message = "";

            if (features.HasFlag(CarFeatures.Bussiness))
            {
                message += "Бизнес, ";
            }

            if (features.HasFlag(CarFeatures.Economy))
            {
                message += "Эконом, ";
            }

            if (features.HasFlag(CarFeatures.Comfort))
            {
                message += "Комфорт, ";
            }

            if (features.HasFlag(CarFeatures.Minivan))
            {
                message += "Минивэн, ";
            }

            return message.Substring(0,message.Length-2);
        }

        private static string GetOrderServices(CarFeatures features)
        {
            var message = "";

            if (features.HasFlag(CarFeatures.Conditioner))
            {
                message += "Кондиционер, ";
            }

            if (features.HasFlag(CarFeatures.Smoke))
            {
                message += "Курить, ";
            }

            if (features.HasFlag(CarFeatures.WithAnimals))
            {
                message += "Животные, ";
            }

            if (features.HasFlag(CarFeatures.StationWagon))
            {
                message += "Универсал, ";
            }

            if (features.HasFlag(CarFeatures.Wifi))
            {
                message += "WiFi, ";
            }

            if (features.HasFlag(CarFeatures.Receipt))
            {
                message += "Квитанция об оплате, ";
            }

            if (features.HasFlag(CarFeatures.Coupon))
            {
                message += "Использовать купон, ";
            }

            return message.Substring(0, message.Length - 2);
        }

        private static string GetChildrenSeatName(ChildrenSeat childrenSeat)
        {
            var name = string.Empty;
            switch (childrenSeat)
            {
                case ChildrenSeat.None:
                    name = "Нет";
                    break;
                case ChildrenSeat.Weight0_10:
                    name = "0 - 1 год, 0 - 10 кг";
                    break;
                case ChildrenSeat.Weight0_13:
                    name = "0 - 1.5 год, 0 - 13 кг";
                    break;
                case ChildrenSeat.Weight0_20:
                    name = "0 - 5 лет, 0 - 20 кг";
                    break;
                case ChildrenSeat.Weight0_25:
                    name = "0 - 7 лет, 0 - 25 кг";
                    break;
                case ChildrenSeat.Weight0_40:
                    name = "0 - 12 лет, 0 - 40 кг";
                    break;
                case ChildrenSeat.Weight9_18:
                    name = "1 - 4 года, 9 - 18 кг";
                    break;
                case ChildrenSeat.Weight9_36:
                    name = "1 - 10 лет, 9 - 36 кг";
                    break;
                case ChildrenSeat.Weight15_25:
                    name = "3 - 7 лет, 15 - 25 кг";
                    break;
                case ChildrenSeat.Weight22_36:
                    name = "6 - 10 лет, 22 - 36 кг";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("childrenSeat");
            }

            return name;
        }
    }
}
