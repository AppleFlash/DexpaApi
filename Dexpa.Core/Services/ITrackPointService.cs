using System;
using System.Collections.Generic;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Tracks;

namespace Dexpa.Core.Services
{
    public interface ITrackPointService : IDisposable
    {
        IList<TrackPoint> GetTrackPoints(long driverId, DateTime dateFrom, DateTime dateTo);

        IList<TrackPoint> GetAggregatedTrackPoints(long driverId, DateTime dateFrom, DateTime dateTo);

        void AddTrackPoint(TrackPoint trackPoint);
        List<DriverTrackPoint> GetDriverTrackPoints(long driverId, DateTime dateFrom, DateTime dateTo);
        IList<TrackPoint> GetDriversPositions(DateTime time);

        TrackData GetTrackerData(long driverId, DateTime dateFrom, DateTime dateTo);
    }
}