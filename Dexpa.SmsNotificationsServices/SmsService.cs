using System;
using System.Linq;
using Dexpa.Core;
using Dexpa.Core.Model;
using Dexpa.Core.Services;
using Dexpa.Ioc;
using Dexpa.ServiceCore;
using Dexpa.SmsGateway;

namespace Dexpa.SmsNotificationsServices
{
    public class SmsService : AService
    {
        private DateTime mOrderStateLastTime;

        private DateTime mDriverReplacedLastTime;

        private ISmsGateway mSmsGateway;

        public SmsService()
        {
            using (var context = new OperationContext())
            {
                var settings = context.GlobalSettingsService.GetSettings();
                mSmsGateway = SmsGatewayFactory.CreateSmsGateway(settings.SmscLogin, settings.SmscPassword);
            }
        }

        protected override void BeforeStart()
        {
            mOrderStateLastTime = DateTime.UtcNow;
            mDriverReplacedLastTime = mOrderStateLastTime;
        }

        protected override void WorkIteration()
        {
            using (var context = new OperationContext())
            {
                ProcessOrderStateChangedEvents(context.EventService);
                ProcessDriverStateChangedEvents(context.EventService);
            }
        }

        private void ProcessDriverStateChangedEvents(IEventService eventService)
        {
            var events = eventService.GetDriverReplacedEvents(mDriverReplacedLastTime);
            foreach (var systemEvent in events)
            {
                try
                {
                    var orderStateType = systemEvent.OrderState;
                    if (systemEvent.Order.Customer != null &&//It's possible for orders from yandex
                        (orderStateType == OrderStateType.Accepted ||
                        orderStateType == OrderStateType.Approved ||
                        orderStateType == OrderStateType.Driving) &&
                        systemEvent.Order.Source != OrderSource.Yandex)
                    {
                        SendDriverReplaced(systemEvent.Order);
                    }
                }
                catch (Exception exception)
                {
                    mLogger.Error("ProcessEventError. Event id: " + systemEvent.Id, exception);
                }
            }
            if (events.Count > 0)
            {
                mDriverReplacedLastTime = events.Last().Timestamp;
            }
        }

        private void ProcessOrderStateChangedEvents(IEventService eventService)
        {
            var events = eventService.GetOrderStateChangedEvents(mOrderStateLastTime);
            foreach (var systemEvent in events)
            {

                try
                {
                    var orderState = systemEvent.OrderState;
                    var order = systemEvent.Order;
                    if (order.Source == OrderSource.Yandex)
                    {
                        continue;
                    }
                    switch (orderState)
                    {
                        case OrderStateType.Created:
                            SendOrderAccepted(order);
                            break;
                        case OrderStateType.Waiting:
                            SendDriverWaiting(order);
                            break;
                        case OrderStateType.Approved:
                            SendOrderAccepted(order);
                            break;
                    }
                }
                catch (Exception exception)
                {
                    mLogger.Error("ProcessEventError. Event id: " + systemEvent.Id, exception);
                }
            }
            if (events.Count > 0)
            {
                mOrderStateLastTime = events.Last().Timestamp;
            }
        }

        private void SendOrderAccepted(Order order)
        {
            mLogger.Debug("Send Accepted to {0} by order {1} ({2})", order.Customer.Phone, order.Id, order.SourceOrderId);
            var message = string.Format("Заказ №{0} принят на исполнение службой Park One", order.Id);
            SendMessage(order, message);
        }

        private void SendMessage(Order order, string message)
        {
            if (order.Source != OrderSource.Yandex)
            {
                var phone = order.Customer.Phone;
                if (IsPhoneCorrect(phone))
                {
                    mSmsGateway.SendMessage(phone, message);
                }
            }
        }

        private bool IsPhoneCorrect(string phone)
        {
            return true;
        }

        private void SendDriverWaiting(Order order)
        {
            mLogger.Debug("Send Driver waiting to {0} by order {1} ({2})", order.Customer.Phone, order.Id, order.SourceOrderId);
            var driver = order.Driver;
            var car = driver.Car;
            var message = string.Format("Вас ожидает {0} на машине {1} {2} {3} {4}. Точное местоположение машины Вы можете уточнить по тел.: {5}.", driver.FirstName, car.Brand, car.Model, car.Color, car.RegNumber, driver.Phones);
            SendMessage(order, message);
        }

        private void SendDriverReplaced(Order order)
        {
            mLogger.Debug("Send Driver replaced to {0} by order {1} ({2})", order.Customer.Phone, order.Id, order.SourceOrderId);
            var name = order.Customer.Name;
            if (string.IsNullOrEmpty(name))
            {
                name = "клиент";
            }
            var driver = order.Driver;
            var car = driver.Car;
            var message = string.Format(" Уважаемый {0} сообщаем Вам, что произведена замена транспортного средства, к вам подьезжает {1} на машине {2} {3} {4} {5}. Точное местоположение машины Вы можете уточнить по тел.: {6}", name, driver.FirstName, car.Brand, car.Model, car.Color, car.RegNumber, driver.Phones);
            SendMessage(order, message);
        }
    }
}
