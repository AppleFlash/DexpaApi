using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dexpa.Core.Services;
using Dexpa.Infrastructure;
using Dexpa.Infrastructure.Repositories;
using Yandex.Taxi.Gateway.Contracts;
using Yandex.Taxi.Gateway.Core;

namespace Dexpa.OrdersGateway
{
    class Program
    {
        static void Main(string[] args)
        {
            string taxiHost = "https://ymsh.taxi-partners-test.mobile.yandex.net";
            string trackUrl = "http://tst.extjams.maps.yandex.net/taxi_collect/1.x/";
            string cid = "1956789018";
            string apiKey = "3efab07c5fda49dd9a6794c3198f06c6";

            var modelContext = new ModelContext();
            var eventRepository = new EventRepository(modelContext);
            var orderRepository = new OrderRepository(modelContext);
            var carRepository = new CarRepository(modelContext);
            var driverRepository = new DriverRepository(modelContext);
            var driverWorkConditionsRepository = new DriverWorkConditionsRepository(modelContext);
            var customerRepository = new CustomerRepository(modelContext);
            var orderHistoryRepository = new OrderHistoryRepository(modelContext);

            var eventService = new EventService(eventRepository, orderRepository);
            var driverService = new DriverService(driverRepository, carRepository, driverWorkConditionsRepository);
            var transactionService = new TransactionService(new TransactionRepository(modelContext), driverRepository);
            var orderService = new OrderService(orderRepository, driverRepository, customerRepository,
                eventRepository, orderHistoryRepository, transactionService);

            var listener = new FakeOrderListener(eventService);

            var gateway = new Gateway(taxiHost, cid, apiKey);
            var tracksGateway = new TracksGateway(trackUrl, cid);

            var orderManager = new OrdersManager(listener, driverService, gateway, orderService);
            orderManager.Start();
            var dataSynchronizer = new DataSynchronizer(gateway, tracksGateway);

            while (true)
            {
                listener.UpdateData();
                Thread.Sleep(5000);
            }
        }
    }
}
