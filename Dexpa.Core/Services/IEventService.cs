using System;
using System.Collections.Generic;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Events;

namespace Dexpa.Core.Services
{
    public interface IEventService : IDisposable
    {
        /// <summary>
        /// Return events after timestamp
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        IList<SystemEvent> GetEvents(DateTime timestamp);

        IList<EventOrderStateChanged> GetOrderStateChangedEvents(DateTime fromTimestamp, long? driverId = null);

        IList<EventOrderStateChanged> GetDriverReplacedEvents(DateTime fromTimestamp);

        IList<SystemEvent> GetOrderEvents(DateTime lastEventTimestamp, long orderId);
    }
}