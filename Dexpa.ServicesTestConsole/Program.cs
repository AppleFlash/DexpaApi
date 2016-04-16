using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dexpa.Core.Services;
using Dexpa.Core.Utils;
using Dexpa.Ioc;
using Dexpa.OrdersGateway;
using Dexpa.ServiceCore;
using Dexpa.SmsNotificationsServices;
using Yandex.Taxi.Gateway.Core;

namespace Dexpa.ServicesTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting sms service...");
            SmsService smsService = new SmsService();
            smsService.Start();

            string clid;
            string apiKey;

            string tracksUrl = ConfigurationManager.AppSettings["TracksUrl"];
            string taxiHost = ConfigurationManager.AppSettings["YandexTaxiHost"];
            int pauseBeforeNextOrder = int.Parse(ConfigurationManager.AppSettings["PauseBeforeNextOrderSec"]);
            int orderRequestAdditionalTimeSec = int.Parse(ConfigurationManager.AppSettings["OrderRequestAdditionalTimeSec"]);

            using (var context = new OperationContext())
            {
                var settings = context.GlobalSettingsService.GetSettings();

                clid = settings.YandexClid;
                apiKey = settings.YandexApiKey;
            }

            var gateway = new Gateway(taxiHost, clid, apiKey);
            var tracksGateway = new TracksGateway(tracksUrl, clid);

            Console.WriteLine("Starting yandex orders service...");
            var yandexService = new YandexTaxiService.YaTaxiService(gateway, pauseBeforeNextOrder, orderRequestAdditionalTimeSec);
            yandexService.Start();

            Console.WriteLine("Starting driver data synchronizer service...");
            var dataSynchronizer = new DataSynchronizer(gateway, tracksGateway);

            Console.WriteLine("Starting cars rent service...");
            var carRentService = new CarRentService();
            carRentService.Start();

            Console.WriteLine("Starting Qiwi parcer service...");
            var qiwiTransactionService = new QiwiTransactionService();
            qiwiTransactionService.Start();

            Console.WriteLine("Starting Yandex Cabinet parser service...");
            string cabUrl = ConfigurationManager.AppSettings["YandexCabinet"];
            var yandexCabinetService = new YandexCabinetParserService(cabUrl);
            yandexCabinetService.Start();

            var balanceWorker = new BalanceRecalculateWorker();
            balanceWorker.Start();

            Console.WriteLine("Starting News Messages sender service...");
            var newsMessagesSendingService = new NewsMessagesSendingService();
            newsMessagesSendingService.Start();

            Console.WriteLine("All services started successful");
            while (true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
