using System;
using System.Collections.Generic;
using System.Threading;
using Dexpa.Core.Model;
using Dexpa.Core.Services;
using Dexpa.Core.Utils;
using Dexpa.Infrastructure;
using Dexpa.Ioc;
using Dexpa.OrdersGateway.Models;
using Dexpa.YandexTaxiService;
using NLog;
using Yandex.Taxi.Gateway.Contracts;
using Yandex.Taxi.Gateway.Contracts.Tracks;
using Point = Yandex.Taxi.Gateway.Contracts.Tracks.Point;

namespace Dexpa.OrdersGateway
{
    public class DataSynchronizer : IDataSynchronizer, IDisposable
    {
        private const int DATA_UPDATE_CHECK_INT_SEC = 5;

        private Logger mLogger = LogManager.GetCurrentClassLogger();

        private IGateway mGateway;

        private ITracksGateway mTracksGateway;

        private Thread mWorkThread;

        private bool mStop;

        private Dictionary<long, Location> mDriversLocations;

        public DataSynchronizer(IGateway gateway, ITracksGateway tracksGateway)
        {
            mLogger.Debug("Created");

            mGateway = gateway;
            mTracksGateway = tracksGateway;

            mDriversLocations = new Dictionary<long, Location>();

            mWorkThread = new Thread(DoWork)
            {
                IsBackground = true
            };
            mWorkThread.Start();
        }

        private IDriverService GetDriverService()
        {
            var scope = new object();
            return IocFactory.Instance.Create<IDriverService>(scope);
        }

        private void DoWork()
        {
            while (!mStop)
            {
                try
                {
                    using (var context = new OperationContext())
                    {
                        var newStates = new List<Driver>();
                        var newLocations = new List<Driver>();

                        var drivers = context.DriverService.GetDrivers(false);
                        foreach (var driver in drivers)
                        {
                            var driverId = driver.Id;

                            Location oldLocation;
                            var newLocation = driver.Location;
                            if (!mDriversLocations.TryGetValue(driverId, out oldLocation))
                            {
                                mDriversLocations.Add(driverId, newLocation);
                                oldLocation = newLocation;
                            }
                            if (!newLocation.Equals(oldLocation))
                            {
                                newLocations.Add(driver);
                                mDriversLocations[driverId] = newLocation;
                                newStates.Add(driver);
                            }
                        }

                        if (newStates.Count > 0 || newLocations.Count > 0)
                        {
                            UpdateDriverData(newStates, newLocations);
                        }
                    }
                }
                catch (ThreadAbortException)
                {
                    //Do nothing
                }
                catch (Exception exception)
                {
                    mLogger.Error(exception);
                }

                Thread.Sleep(DATA_UPDATE_CHECK_INT_SEC * 1000);
            }
        }

        private void UpdateDriverData(List<Driver> newStates, List<Driver> newLocations)
        {
            mLogger.Debug("UpdateDriverData: newStates {0}, newLocations {1}", newStates.Count, newLocations.Count);
            var yaStates = new List<DriverStatus>();
            foreach (var driver in newStates)
            {
                var status = MapDriverStatus(driver.State);
                var driverStatus = new DriverStatus(driver.Uuid, status);
                yaStates.Add(driverStatus);
            }

            var yaTracks = new List<ITrack>();
            foreach (var driver in newLocations)
            {
                var location = driver.Location;
                var point = new Point(
                    (decimal)location.Latitude,
                    (decimal)location.Longitude,
                    (int)location.Speed,
                    (int)location.Direction,
                    DateTime.UtcNow);
                var track = new YTrack
                {
                    Uuid = driver.Uuid,
                    Points = new List<Point>(new[] { point })
                };
                yaTracks.Add(track);
            }

            var trackList = new YTracks(yaTracks);

            if (newStates.Count > 0)
            {
                mGateway.SendDriversStatus(yaStates.ToArray());
            }
            if (newLocations.Count > 0)
            {
                mTracksGateway.SendTracks(trackList);
            }
        }

        private Status MapDriverStatus(DriverState state)
        {
            switch (state)
            {
                case DriverState.Busy:
                    return Status.Busy;
                case DriverState.NotAvailable:
                case DriverState.Blocked:
                    return Status.VeryBusy;
                case DriverState.ReadyToWork:
                    return Status.Free;
            }
            return Status.VeryBusy;
        }

        public IDriversProfile RequestDriversProfile()
        {
            /* var drivers = mDriverService.GetDrivers();
             var workingDrivers = drivers
                 .Where(d => d.State != DriverState.Fired)
                 .ToList();

             return ObjectMapper.Instance.Map<List<Driver>, YDriversProfiles>(workingDrivers);*/
            return null;
        }

        public IBlacklist RequestBlacklist()
        {
            return new YBlacklist();
        }

        public void Dispose()
        {
            mStop = true;
            mWorkThread.Join();
        }
    }
}
