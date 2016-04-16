using System;
using System.Collections.Generic;
using System.Linq;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Events;
using Dexpa.Core.Repositories;

namespace Dexpa.Core.Services
{
    public class EventService : IEventService
    {
        private IEventRepository mEventRepository;

        private IOrderRepository mOrderRepository;

        public EventService(IEventRepository eventRepository, IOrderRepository orderRepository)
        {
            mEventRepository = eventRepository;
            mOrderRepository = orderRepository;
        }

        /// <summary>
        /// Return events after timestamp
        /// </summary>
        /// <param name="fromTimestamp"></param>
        /// <returns></returns>
        public IList<SystemEvent> GetEvents(DateTime fromTimestamp)
        {
            return mEventRepository.List(e => e.Timestamp > fromTimestamp);
        }

        public IList<EventOrderStateChanged> GetOrderStateChangedEvents(DateTime fromTimestamp, long? driverId = null)
        {
            var events = mEventRepository.List(e => e.Timestamp > fromTimestamp && e.Type == EventType.OrderStateChanged);
            var orderIds = events
                .Select(e => e.RelatedItemId)
                .Distinct()
                .ToList();
            var orders = mOrderRepository
                .List(o => orderIds.Contains(o.Id) &&
                (!driverId.HasValue || o.Driver.Id == driverId));

            var orderEvents = new List<EventOrderStateChanged>();
            for (int i = 0; i < events.Count; i++)
            {
                var ev = events[i];
                var order = orders.FirstOrDefault(o => o.Id == ev.RelatedItemId);

                if (order != null) //is driver match
                {
                    var orderEvent = new EventOrderStateChanged
                    {
                        Id = ev.Id,
                        Timestamp = ev.Timestamp,
                        Order = order,
                        OrderState = ev.OrderState
                    };

                    orderEvents.Add(orderEvent);
                }
            }

            return orderEvents;
        }

        public IList<EventOrderStateChanged> GetDriverReplacedEvents(DateTime fromTimestamp)
        {
            var events = mEventRepository.List(e => e.Timestamp > fromTimestamp && e.Type == EventType.DriverReplaced);
            var orderIds = events
                .Select(e => e.RelatedItemId)
                .Distinct()
                .ToList();

            var orders = mOrderRepository
                .List(o => orderIds.Contains(o.Id));

            var orderEvents = new List<EventOrderStateChanged>();
            for (int i = 0; i < events.Count; i++)
            {
                var ev = events[i];
                var order = orders.FirstOrDefault(o => o.Id == ev.RelatedItemId);

                if (order != null) //is driver match
                {
                    var orderEvent = new EventOrderStateChanged
                    {
                        Id = ev.Id,
                        Timestamp = ev.Timestamp,
                        Order = order,
                        OrderState = ev.OrderState
                    };

                    orderEvents.Add(orderEvent);
                }
            }

            return orderEvents;
        }

        public IList<SystemEvent> GetOrderEvents(DateTime lastEventTimestamp, long orderId)
        {
            return mEventRepository.List(e => e.Timestamp > lastEventTimestamp && e.RelatedItemId == orderId);
        }

        public void Dispose()
        {
            mOrderRepository.Dispose();
            mEventRepository.Dispose();
        }
    }
}
