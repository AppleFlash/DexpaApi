using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Tracks;
using Dexpa.Core.Repositories;
namespace Dexpa.Core.Services
{
    public class TrackPointService : ITrackPointService
    {
        private ITrackPointRepository mPointRepository;

        private IOrderRepository mOrderRepository;

        private IRobotLogRepository mRobotLogRepository;

        public TrackPointService(ITrackPointRepository pointRepository,
            IOrderRepository orderRepository, IRobotLogRepository robotLogRepository)
        {
            mPointRepository = pointRepository;
            mOrderRepository = orderRepository;
            mRobotLogRepository = robotLogRepository;
        }

        public IList<TrackPoint> GetTrackPoints(long driverId, DateTime dateFrom, DateTime dateTo)
        {
            return mPointRepository.List(t => t.DriverId == driverId &&
                t.Timestamp >= dateFrom && t.Timestamp <= dateTo);
        }

        public IList<TrackPoint> GetAggregatedTrackPoints(long driverId, DateTime dateFrom, DateTime dateTo)
        {
            //Temporary disabled aggregation
             var points = GetTrackPoints(driverId, dateFrom, dateTo);
            return points;
            var aggregatedPoints = new List<TrackPoint>();
            const int maxPointsToAggregate = 6;
            const int maxSecondsBtwPoints = 20;
            double sumSpeed = 0, sumDirection = 0;
            int pointCount = 0;
            for (int i = 0; i < points.Count - 1; i++)
            {
                pointCount++;
                var point = points[i];
                var nextPoint = points[i + 1];
                var span = nextPoint.Timestamp - point.Timestamp;
                sumDirection += point.Direction;
                sumSpeed += point.Speed;
                if (span.TotalSeconds > maxSecondsBtwPoints || maxPointsToAggregate == pointCount)
                {
                    var agrPoint = new TrackPoint
                    {
                        Timestamp = point.Timestamp,
                        Direction = (int)(sumDirection / pointCount),
                        Longitude = point.Longitude,
                        Latitude = point.Latitude,
                        Id = point.Id,
                        DriverId = point.DriverId,
                        Driver = point.Driver,
                        Speed = sumSpeed / pointCount
                    };
                    aggregatedPoints.Add(agrPoint);
                    pointCount = 0;
                    sumSpeed = 0;
                    sumDirection = 0;
                }
            }
            return aggregatedPoints;
        }


        public List<DriverTrackPoint> GetDriverTrackPoints(long driverId, DateTime dateFrom, DateTime dateTo)
        {
            var points = GetTrackPoints(driverId, dateFrom, dateTo);
            if (points.Count == 0) return null;
            const double limDistance = 1.5,
                         degree = 20,
                         totalMinBetween2Points = 10;
            double x1 = 0,
                   y1 = 0,
                   x2 = 0,
                   y2 = 0,
                   sumDist = 0;
            int count;
            var aggregatedPoints = new List<DriverTrackPoint>();
            /*x1 = points[1].Latitude - points[0].Latitude;
            y1 = points[1].Longitude - points[0].Longitude;*/
            //aggregatedPoints.Add(AddAgregatePoint(points[0], AggregatedPointService.DirectionToString(points[0].Direction)));

            for (int i = 0; i < points.Count - 1; i++)
            {
               // double averegeDirection = 0;
                //double averegeDistance = 0;
                double angle = 0;
                var point = points[i];
                var nextPoint = points[i+1];
                var lat3Point = 0.0;
                var long3Point = 0.0;
                if(i+2 < points.Count)
                {
                    var nextPoint2 = points[i + 2];
                    lat3Point = nextPoint2.Latitude;
                    long3Point = nextPoint2.Longitude;

                }
                var lat1Point = point.Latitude;
                var long1Point = point.Longitude;
                var lat2Point = nextPoint.Latitude;
                var long2Point = nextPoint.Longitude;
                
                string direction = AggregatedPointService.DirectionToString(nextPoint.Direction);
                var deltaTime = nextPoint.Timestamp - point.Timestamp;
                var distanceBetween2points = Utils.Utils.GetDistance(lat1Point, long1Point, lat2Point, long2Point);

                var tSpeed = (distanceBetween2points*1000 / deltaTime.Seconds)*3.6;
               if (tSpeed < 150 || deltaTime.Seconds == 0)
                {
                    sumDist += (double) distanceBetween2points;
                }

               /*if (i > 0)
               {
                    x2 = lat2Point - lat1Point;
                    y2 = long2Point - long1Point;
                    angle = AggregatedPointService.CalculateAngle(x1, y1, x2, y2);
                    x1 = x2;
                    y1 = y2;
                }*/
                x1 = lat2Point - lat1Point;
                y1 = long2Point - long1Point;
                x2 = lat3Point - lat1Point;
                y2 = long3Point - long1Point;
                angle = AggregatedPointService.CalculateAngle(x1, y1, x2, y2);

                TrackPoint areaPoint = AggregatedPointService.AreaPoints(points, i, out count);
                i += count;
                
                if (nextPoint.Speed < 8 || tSpeed > 150)
                {
                    continue;
                }
                else if (areaPoint != null)
                {
                    aggregatedPoints.Add(AddAgregatePoint(areaPoint, direction));
                    sumDist = 0;
                }
                else if (sumDist >= limDistance)
                {
                    aggregatedPoints.Add(AddAgregatePoint(nextPoint, direction));
                    sumDist = 0;
                }

                else if (deltaTime.TotalMinutes > totalMinBetween2Points)
                {
                    aggregatedPoints.Add(AddAgregatePoint(point, direction));
                    sumDist = 0;
                }

                else if (angle > degree && i > 0 && tSpeed < 150)
                {
                    aggregatedPoints.Add(AddAgregatePoint(point, direction));
                    sumDist = 0;
                }

                else if (point.DriverState != nextPoint.DriverState)
                {
                    aggregatedPoints.Add(AddAgregatePoint(point, direction));
                    sumDist = 0;
                }

            }
            return aggregatedPoints;
        }

        public void AddTrackPoint(TrackPoint trackPoint)
        {
            mPointRepository.Add(trackPoint);
            mPointRepository.Commit();
        }

        /// <summary>
        /// Get drivers positions an the Time
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public IList<TrackPoint> GetDriversPositions(DateTime time)
        {
            var timeRangeMins = 5;
            var fromTime = time.AddMinutes(-timeRangeMins);
            var toTime = time;

            var points = mPointRepository.List(p => p.Timestamp >= fromTime && p.Timestamp <= toTime);

            var pointsByDriver = points
                .GroupBy(p => p.DriverId)
                .Select(gr =>
                    gr.OrderBy(p => p.Timestamp).FirstOrDefault())
                .ToList();

            return pointsByDriver;
        }

        public TrackData GetTrackerData(long driverId, DateTime dateFrom, DateTime dateTo)
        {
            var points = mPointRepository.List(t => t.DriverId == driverId &&
                t.Timestamp >= dateFrom && t.Timestamp <= dateTo);
            //Заказы, которые выполнил (выполняет) водитель
            var orders = mOrderRepository.List(o => o.DriverId == driverId &&
                o.Timestamp >= dateFrom && o.Timestamp <= dateTo); 

            var potOrdersIds = mRobotLogRepository.List(s => s.DriverId == driverId &&
                s.Timestamp >= dateFrom && s.Timestamp <= dateTo && s.IsDriverSelected)
                .Select(s => s.OrderId)
                .ToList();

            

            //Заказы для Новый заказ (белый)
            var potOrders = mOrderRepository.List(o => potOrdersIds.Contains(o.Id));
            var driverPoints = GetDriverTrackPoints(driverId, dateFrom, dateTo);



            var trackData = TestMethod(orders, driverPoints);
            return trackData;

            //points[0].Speed;
            //points[0].Direction;
            //С - 0
            //В - 90
            //points[0].DriverState == DriverState.NotAvailable

            //return null;
        }

        public void Dispose()
        {
            mPointRepository.Dispose();
        }

       

        public TrackData TestMethod(IList<Order> orders, List<DriverTrackPoint> driverPoints)
        {
            var onOrderPointsList = new List<OnOrderPoint>();
            var waitingClientList = new List<WaitingClientPoint>();
            var orderPointsList = new List<OrderPoint>();
            orders = orders.OrderBy(o => o.DepartureDate).ToList();
            driverPoints = driverPoints.OrderBy(p => p.Timestamp).ToList();
            int count = 0;
            for (int i = count; i < orders.Count-1; i++)
            {
                var drivingHistory = orders[i].OrderHistories.FirstOrDefault(h => h.OrderState == OrderStateType.Driving);
                var waitingHistory = orders[i].OrderHistories.FirstOrDefault(h => h.OrderState == OrderStateType.Waiting);
                var tranportingHistory = orders[i].OrderHistories.FirstOrDefault(h => h.OrderState == OrderStateType.Transporting);
                var complitedHistory = orders[i].OrderHistories.FirstOrDefault(h => h.OrderState == OrderStateType.Completed);
                var failedHistory = orders[i].OrderHistories.FirstOrDefault(h => h.OrderState == OrderStateType.Failed);
                var canceledHistory = orders[i].OrderHistories.FirstOrDefault(h => h.OrderState == OrderStateType.Canceled);

                var sortedOrders = SortOrdersByTimestamp(drivingHistory ,waitingHistory, tranportingHistory, complitedHistory, failedHistory, canceledHistory);
                int k = 0;
                
                for (int j = count; j < driverPoints.Count - 1; j++)
                {
                    if(k >= sortedOrders.Count-1) break;

                    if (drivingHistory != null && (driverPoints[j].Timestamp < drivingHistory.Timestamp &&
                                                   driverPoints[j + 1].Timestamp > drivingHistory.Timestamp))
                    {
                        while (k < sortedOrders.Count && j < driverPoints.Count - 1)
                        {
                            if (k + 1 < sortedOrders.Count)
                            {
                                while (!(driverPoints[j].Timestamp < sortedOrders[k + 1].Timestamp &&
                                            driverPoints[j + 1].Timestamp > sortedOrders[k + 1].Timestamp))
                                {
                                    if (sortedOrders[k] == drivingHistory)
                                    {
                                        onOrderPointsList.Add(ModelOnOrderPoint(driverPoints[j], drivingHistory));
                                        orderPointsList.Add(ModelOrderPoint(driverPoints[j], drivingHistory));
                                        driverPoints[j].PointType = TrackPointType.DrivingToClient;
                                        //Console.WriteLine(j + ") Driving\n");
                                    }
                                    else if (sortedOrders[k] == waitingHistory)
                                    {
                                        waitingClientList.Add(ModelWaitingPoints(driverPoints[j], waitingHistory));
                                        orderPointsList.Add(ModelOrderPoint(driverPoints[j], waitingHistory));
                                        driverPoints[j].PointType = TrackPointType.WaitingClient;
                                       // Console.WriteLine(j + ") Waiting\n");
                                    }
                                    else if (sortedOrders[k] == tranportingHistory)
                                    {
                                        onOrderPointsList.Add(ModelOnOrderPoint(driverPoints[j], tranportingHistory));
                                        orderPointsList.Add(ModelOrderPoint(driverPoints[j], tranportingHistory));
                                        driverPoints[j].PointType = TrackPointType.TransportingClient;
                                        //Console.WriteLine(j + ") Transporting\n");
                                    }

                                    j++;
                                }
                            }
                            else
                            {
                                if (sortedOrders[k] == canceledHistory)
                                {
                                    driverPoints[j].PointType = TrackPointType.OrderCancelled;
                                    //Console.WriteLine(j + ") Cancel\n");
                                    //создание модели для окончания заказа
                                }
                                else if (sortedOrders[k] == failedHistory)
                                {
                                    driverPoints[j].PointType = TrackPointType.OrderFailed;
                                    //Console.WriteLine(j + ") Failed\n");
                                }
                                else if (sortedOrders[k] == complitedHistory)
                                {
                                    onOrderPointsList.Add(ModelOnOrderPoint(driverPoints[j], complitedHistory));
                                    driverPoints[j].PointType = TrackPointType.OrderCompleted;
                                    //Console.WriteLine(j + ") Complited\n");
                                }
                            }
                            ++k;
                        }
                    }
                    count++;
                }
            }
            return new TrackData()
            {
                Points = driverPoints,
                OnOrderPoints = onOrderPointsList,
                WaitingClientPoints = waitingClientList,
                OrderPoints = orderPointsList
            };
        }

        public List<OrderHistory> SortOrdersByTimestamp(OrderHistory driving, OrderHistory waiting, OrderHistory transport, OrderHistory compl, OrderHistory fail, OrderHistory cancel)
        {
            List<DateTime> mass = new List<DateTime>();
            List<OrderHistory> or = new List<OrderHistory>();
            if (driving != null) mass.Add(driving.Timestamp);
            if (waiting != null) mass.Add(waiting.Timestamp);
            if (transport != null) mass.Add(transport.Timestamp);
            if (compl != null) mass.Add(compl.Timestamp);
            if (fail != null) mass.Add(fail.Timestamp);
            if (cancel != null) mass.Add(cancel.Timestamp);
            mass.Sort();
            for (int i = 0; i < mass.Count; i++)
            {
                if (driving != null && mass[i] == driving.Timestamp)
                    or.Add(driving);
                if (waiting != null && mass[i] == waiting.Timestamp)
                    or.Add(waiting);
                if (transport != null && mass[i] == transport.Timestamp)
                    or.Add(transport);
                if (compl != null && mass[i] == compl.Timestamp)
                    or.Add(compl);
                if (fail != null && mass[i] == fail.Timestamp)
                    or.Add(fail);
                if (cancel != null && mass[i] == cancel.Timestamp)
                    or.Add(cancel);
            }
            return or.ToList();
        }
        public DriverTrackPoint AddAgregatePoint(TrackPoint point, string direction)
        {
            var agr = new DriverTrackPoint
            {
                Id = point.Id,
                Timestamp = point.Timestamp,
                Latitude = point.Latitude,
                Longitude = point.Longitude,
                Direction = direction,
                Speed = (int)point.Speed,
                PointType = TrackPointType.Driving,
                DriverState = point.DriverState
            };
            return agr;
        }
        public OnOrderPoint ModelOnOrderPoint(DriverTrackPoint point, OrderHistory history)
        {
            return new OnOrderPoint()
            {
                OrderId = history.OrderId,
                PointId = point.Id,
                Cost = 30,
                DistanceCity = 3,
                DistanceOutCity = 3,
                TimeCity = 3,
                TimeOutCity = 4
            };
        }

        public WaitingClientPoint ModelWaitingPoints(DriverTrackPoint point, OrderHistory history)
        {
            return new WaitingClientPoint()
            {
                OrderId = history.OrderId,
                PointId = point.Id,
                WaitingCost = 0,
                PaidWaiting = 0,
                FreeWaiting = 0,
                Delay = 0
            };
        }

        public OrderPoint ModelOrderPoint(DriverTrackPoint point, OrderHistory order)
        {
            return new OrderPoint()
            {
                OrderId = order.OrderId,
                PointId = point.Id,
                Timestamp = point.Timestamp,
                FromAddress = order.Order.FromAddress.Street + " " + order.Order.FromAddress.House,
                ToAddress = order.Order.ToAddress.Street + " " + order.Order.ToAddress.House,
                Date = order.Timestamp,
                Latitude = point.Latitude,
                Longitude = point.Longitude
            };
        }
    }
}


