using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Dexpa.Core.Model.Events;
using Dexpa.Core.Services;
using Dexpa.DTO.Events;
using Dexpa.WebApi.Utils;

namespace Dexpa.WebApi.Controllers
{
    public class EventsController : ApiControllerBase
    {
        private IEventService mEventService;

        public EventsController(IEventService eventService)
        {
            mEventService = eventService;
        }

        [HttpGet]
        public List<EventOrderStateChangedDTO> GetOrderStateChanged(DateTime fromTimestamp)
        {
            var utcTimestamp = TimeHelper.LocalToUtc(fromTimestamp);
            var events = mEventService.GetOrderStateChangedEvents(utcTimestamp);
            return ObjectMapper.Instance.Map<IList<EventOrderStateChanged>, List<EventOrderStateChangedDTO>>(events);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mEventService.Dispose();
            }

            base.Dispose(disposing);
        }
	}
}