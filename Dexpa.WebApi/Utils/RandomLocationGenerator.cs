using System;
using System.Linq;
using System.Threading;
using Dexpa.Core.Model;
using Dexpa.Core.Services;

namespace Dexpa.WebApi.Utils
{
    public class RandomLocationGenerator
    {
        private const double mLongMin = 36.826137;

        private const double mLongMax = 38.314784;

        private const double mLatMin = 55.31687;

        private const double mLatMax = 56.175112;

        private const double mSpeedMin = 5;

        private const double mSpeedMax = 90;

        private const int mPositionUpdateTimeMs = 5000;

        private volatile bool mGenerationActive;

        private Thread mWorkThread;

        private Random mRandom;

        private IDriverService mDriverService;

        public RandomLocationGenerator(IDriverService driverService)
        {
            mRandom = new Random();
            mDriverService = driverService;
        }

        public void Start()
        {
            if (!mGenerationActive)
            {
                mGenerationActive = true;
                mWorkThread = new Thread(Generate);
                mWorkThread.IsBackground = true;
                mWorkThread.Start();
            }
        }

        private void Generate()
        {
            try
            {
                var drivers = mDriverService.GetDrivers();
                drivers = drivers
                    .Where(d => d.State != DriverState.Fired)
                    .ToList();

                while (mGenerationActive)
                {
                    foreach (var driver in drivers)
                    {
                        driver.Location = new Location
                        {
                            Direction = GetRandomValue(0, 360),
                            Latitude = GetRandomValue(mLatMin, mLatMax),
                            Longitude = GetRandomValue(mLongMin, mLongMax),
                            Speed = GetRandomValue(mSpeedMin, mSpeedMax)
                        };
                        mDriverService.UpdateDriver(driver);
                    }
                    Thread.Sleep(mPositionUpdateTimeMs);
                }
            }
            catch
            {
            }
        }

        private double GetRandomValue(double minValue, double maxValue)
        {
            return mRandom.NextDouble() * (maxValue - minValue) + minValue;
        }

        public void Stop()
        {
            if (mGenerationActive)
            {
                mGenerationActive = false;
                mWorkThread.Join();
            }
        }
    }
}