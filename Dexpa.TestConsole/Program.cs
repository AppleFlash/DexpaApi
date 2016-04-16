using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dexpa.Core.Factories;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;
using Dexpa.Core.Services;
using Dexpa.Core.Utils;
using Dexpa.Infrastructure;
using Dexpa.Infrastructure.Repositories;
using Dexpa.Ioc;
using Dexpa.YandexCabinet.Parser;

namespace Dexpa.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
             /*var scope = new object();
            var orderService = Ioc.IocFactory.Instance.Create<IOrderService>(scope);
            var order = orderService.GetOrder(10199);
            var driverService = IocFactory.Instance.Create<IDriverService>(scope);
            var driver = driverService.GetDriver(302);
            var requestService = IocFactory.Instance.Create<IDriverOrderRequestService>(scope);
            requestService.AddRequest(order, driver, OrderRequestState.Rejected);
            var scope = new object();
            var scope = new object();
            var settingsService = Ioc.IocFactory.Instance.Create<IGlobalSettingsService>(scope);
            var settings = settingsService.GetSettings();
            var id = settings.YandexCabId;
            if (settings.YandexCabLogin != null && settings.YandexCabPassword != null)
            {
                var parser = new YandexCabinetParser(settings.YandexCabLogin, settings.YandexCabPassword,
                settings.YandexCabId);
            }
            string fileName = "RegionPoints";
            var storage = new RegionBinaryStorage(fileName);
            storage.GetAllPoints();*/

            var scope = new object();
            var trackService = Ioc.IocFactory.Instance.Create<ITrackPointService>(scope);
            DateTime s1 = Convert.ToDateTime("01.01.2015 21:56 ");
            DateTime s2 = Convert.ToDateTime("14.01.2015 21:55");
            //Stopwatch watch = new Stopwatch();
            //watch.Start();
            trackService.GetTrackerData(363, s1, s2);
            Console.ReadKey();
            /*watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds.ToString());
            Console.ReadKey();*/
        }
    }
}
