using System;
using System.Collections.Generic;
using Dexpa.Core.Model;
using Dexpa.Core.Services;
using Dexpa.Core.Utils;
using Dexpa.DTO;
using Dexpa.WebApi.Utils;

namespace Dexpa.WebApi.Controllers
{
    public class TrackPointsController : ApiControllerBase
    {
        private ITrackPointService mTrackPointService;

        public TrackPointsController(ITrackPointService trackPointService)
        {
            mTrackPointService = trackPointService;
        }

        public IList<TrackPointDTO> Get(long driverId, DateTime fromDate, DateTime toDate)
        {
           /* var fromDateUtc = TimeConverter.LocalToUtc(fromDate);
            var toDateUtc = TimeConverter.LocalToUtc(toDate);
            var point = mTrackPointService.GetAggregatedTrackPoints(driverId, fromDateUtc, toDateUtc);
            var pointDTOs = ObjectMapper.Instance.Map<IList<TrackPoint>, List<TrackPointDTO>>(point);*/
            return null;
        }

        public IList<TrackPointDTO> GetDriverPositions(DateTime time)
        {
            var timeUtc = TimeConverter.LocalToUtc(time);
            var point = mTrackPointService.GetDriversPositions(timeUtc);
            var pointDTOs = ObjectMapper.Instance.Map<IList<TrackPoint>, List<TrackPointDTO>>(point);
            return pointDTOs;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mTrackPointService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}