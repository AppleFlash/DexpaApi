using System;
using System.Collections.Generic;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Tracks;
using Dexpa.Core.Services;

namespace Dexpa.WebApi.Controllers
{
    public class TrackerController : ApiControllerBase
    {
        private ITrackPointService mTrackPointService;

        public TrackerController(ITrackPointService trackPointService)
        {
            mTrackPointService = trackPointService;
        }

        public TrackData Get(long driverId, DateTime fromDate, DateTime toDate)
        {
            var data = new TrackData
            {
                /*Points = GetPoints(),
                OnOrderPoints = GetOnOrderPoints(),
                OrderPoints = GetOrderPoints(),
                WaitingClientPoints = GetWaitingClientPoints()*/
            };

           // return data;

            return mTrackPointService.GetTrackerData(driverId, fromDate, toDate);
        }

        private List<WaitingClientPoint> GetWaitingClientPoints()
        {
            return new List<WaitingClientPoint>
            {
                new WaitingClientPoint
                {
                    FreeWaiting = 3,
                    PaidWaiting = 5,
                    WaitingCost = 50,
                    PointId = 3,
                    OrderId = 1,
                    Delay = 5
                },
                new WaitingClientPoint
                {
                    FreeWaiting = 3,
                    PaidWaiting = 5,
                    WaitingCost = 50,
                    PointId = 7,
                    OrderId = 2,
                    Delay = -3
                }
            };
        }

        private List<OrderPoint> GetOrderPoints()
        {
            return new List<OrderPoint>
            {
                new OrderPoint
                {
                    Date = new DateTime(2014, 1, 1, 10, 15, 30),
                    FromAddress = "Красная площадь 1",
                    ToAddress = "Домодедово",
                    OrderId = 1,
                    PointId = 1
                },
                new OrderPoint
                {
                    Date = new DateTime(2014, 1, 1, 10, 15, 30),
                    FromAddress = "Красная площадь 15",
                    ToAddress = "Домодедово",
                    OrderId = 2,
                    PointId = 5
                },
                new OrderPoint
                {
                    Date = new DateTime(2014, 1, 1, 10, 15, 30),
                    FromAddress = "проспект Мира 27",
                    ToAddress = "Домодедово",
                    Latitude = 55.760265,
                    Longitude = 37.550802,
                    OrderId = 3,
                    PointId = null,
                    Timestamp = DateTime.Now
                },
                new OrderPoint
                {
                    Date = new DateTime(2014, 1, 1, 10, 15, 30),
                    FromAddress = "проспект Мира 13",
                    ToAddress = "Домодедово",
                    Latitude = 55.740265,
                    Longitude = 37.530802,
                    OrderId = 4,
                    PointId = null,
                    Timestamp = DateTime.Now
                },
                new OrderPoint
                {
                    Date = new DateTime(2014, 1, 1, 10, 15, 30),
                    FromAddress = "проспект Мира 102",
                    ToAddress = "Домодедово",
                    OrderId = 5,
                    PointId = 12
                }
            };
        }

        private List<OnOrderPoint> GetOnOrderPoints()
        {
            return new List<OnOrderPoint>
            {
                new OnOrderPoint
                {
                    Cost = 50,
                    DistanceCity = 5.4,
                    DistanceOutCity = 0,
                    OrderId = 1,
                    PointId = 2,
                    TimeCity = 10,
                    TimeOutCity = 0
                },
                new OnOrderPoint
                {
                    Cost = 50,
                    DistanceCity = 5.4,
                    DistanceOutCity = 0,
                    OrderId = 2,
                    PointId = 6,
                    TimeCity = 10,
                    TimeOutCity = 0
                },
                new OnOrderPoint
                {
                    Cost = 50,
                    DistanceCity = 5.4,
                    DistanceOutCity = 0,
                    OrderId = 2,
                    PointId = 8,
                    TimeCity = 10,
                    TimeOutCity = 0
                },
                new OnOrderPoint
                {
                    Cost = 50,
                    DistanceCity = 5.4,
                    DistanceOutCity = 0,
                    OrderId = 2,
                    PointId = 9,
                    TimeCity = 10,
                    TimeOutCity = 0
                }
            };
        }

        private List<DriverTrackPoint> GetPoints()
        {
            return new List<DriverTrackPoint>
            {
                new DriverTrackPoint
                {
                    Id = 1,
                    Direction = "СВ",
                    DriverState = DriverState.ReadyToWork,
                    Latitude = 55.790265,
                    Longitude = 37.590802,
                    PointType = TrackPointType.Driving,
                    Speed = 66,
                    Timestamp = DateTime.Now
                },
                new DriverTrackPoint
                {
                    Id = 2,
                    Direction = "СВ",
                    DriverState = DriverState.Busy,
                    Latitude = 55.790265,
                    Longitude = 37.600802,
                    PointType = TrackPointType.TransportingClient,
                    Speed = 66,
                    Timestamp = DateTime.Now
                },
                new DriverTrackPoint
                {
                    Id = 3,
                    Direction = "СВ",
                    DriverState = DriverState.Busy,
                    Latitude = 55.790265,
                    Longitude = 37.610802,
                    PointType = TrackPointType.WaitingClient,
                    Speed = 66,
                    Timestamp = DateTime.Now
                },
                new DriverTrackPoint
                {
                    Id = 4,
                    Direction = "СВ",
                    DriverState = DriverState.NotAvailable,
                    Latitude = 55.800275,
                    Longitude = 37.620802,
                    PointType = TrackPointType.Driving,
                    Speed = 66,
                    Timestamp = DateTime.Now
                },
                new DriverTrackPoint
                {
                    Id = 5,
                    Direction = "СВ",
                    DriverState = DriverState.ReadyToWork,
                    Latitude = 55.810285,
                    Longitude = 37.620802,
                    PointType = TrackPointType.NewOrder,
                    Speed = 66,
                    Timestamp = DateTime.Now
                },
                new DriverTrackPoint
                {
                    Id = 6,
                    Direction = "СВ",
                    DriverState = DriverState.Busy,
                    Latitude = 55.820295,
                    Longitude = 37.620802,
                    PointType = TrackPointType.DrivingToClient,
                    Speed = 66,
                    Timestamp = DateTime.Now
                },
                new DriverTrackPoint
                {
                    Id = 7,
                    Direction = "СВ",
                    DriverState = DriverState.Busy,
                    Latitude = 55.830305,
                    Longitude = 37.620802,
                    PointType = TrackPointType.WaitingClient,
                    Speed = 66,
                    Timestamp = DateTime.Now
                },
                new DriverTrackPoint
                {
                    Id = 8,
                    Direction = "СВ",
                    DriverState = DriverState.Busy,
                    Latitude = 55.840315,
                    Longitude = 37.620802,
                    PointType = TrackPointType.TransportingClient,
                    Speed = 66,
                    Timestamp = DateTime.Now
                },
                new DriverTrackPoint
                {
                    Id = 9,
                    Direction = "СВ",
                    DriverState = DriverState.ReadyToWork,
                    Latitude = 55.850330,
                    Longitude = 37.620802,
                    PointType = TrackPointType.OrderCompleted,
                    Speed = 66,
                    Timestamp = DateTime.Now
                },
                new DriverTrackPoint
                {
                    Id = 10,
                    Direction = "СВ",
                    DriverState = DriverState.NotAvailable,
                    Latitude = 55.950330,
                    Longitude = 37.620802,
                    PointType = TrackPointType.Idle,
                    Speed = 66,
                    Timestamp = DateTime.Now
                },
                new DriverTrackPoint
                {
                    Id = 11,
                    Direction = "СВ",
                    DriverState = DriverState.ReadyToWork,
                    Latitude = 55.950330,
                    Longitude = 37.620802,
                    PointType = TrackPointType.Idle,
                    Speed = 66,
                    Timestamp = DateTime.Now
                },
                new DriverTrackPoint
                {
                    Id = 12,
                    Direction = "СВ",
                    DriverState = DriverState.ReadyToWork,
                    Latitude = 55.950330,
                    Longitude = 37.620802,
                    PointType = TrackPointType.NewOrder,
                    Speed = 66,
                    Timestamp = DateTime.Now
                },
                new DriverTrackPoint
                {
                    Id = 13,
                    Direction = "СВ",
                    DriverState = DriverState.NotAvailable,
                    Latitude = 55.790265,
                    Longitude = 37.590802,
                    PointType = TrackPointType.Driving,
                    Speed = 66,
                    Timestamp = DateTime.Now
                }
            };
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