using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Yandex.Taxi.Gateway.Contracts;
using Yandex.Taxi.Model.Statuses;
using YAXLib;

namespace Yandex.Taxi.Gateway.Core
{
    public class Gateway : IGateway
    {
        private readonly string mTaxiHost;

        private readonly string mClid;

        private readonly string mApiKey;

        private readonly YAXSerializer mCarsStatusSerializer;

        private readonly Logger mLogger = LogManager.GetCurrentClassLogger();

        public Gateway(string sTaxiHost, string sClid, string sApiKey)
        {
            this.mTaxiHost = sTaxiHost;
            this.mClid = sClid;
            this.mApiKey = sApiKey;
            this.mCarsStatusSerializer = new YAXSerializer(typeof(CarsStatus));
        }

        public void SendDriversStatus(params DriverStatus[] statuses)
        {
            CarsStatus carsStatus = Map(statuses);

            string sСarsStatus = mCarsStatusSerializer.Serialize(carsStatus);

            using (HttpClient client = CreateHttpClient())
            {
                string sURI = string.Format("{0}/1.x/carstatus?clid={1}&apikey={2}", mTaxiHost, mClid, mApiKey);

                mLogger.Debug("SendDriversStatus: {0}", sURI);
                // TODO: Reliable POST
                var result = client.PostAsync(sURI, new StringContent(sСarsStatus)).Result;

                mLogger.Debug("SendDriversStatus. Result: {0}", result);
            }
        }

        private static CarsStatus Map(DriverStatus[] statuses)
        {
            CarsStatus result = new CarsStatus();
            foreach (var driverStatus in statuses)
            {
                result.Cars.Add(Map(driverStatus));
            }
            return result;
        }

        private static Car Map(DriverStatus driverStatus)
        {
            Car result = new Car();
            result.Uuid = driverStatus.Uuid;
            result.Status = Map(driverStatus.Status);
            return result;
        }

        private static Model.Statuses.Status Map(Contracts.Status eStatus)
        {
            switch (eStatus)
            {
                case Yandex.Taxi.Gateway.Contracts.Status.Free:
                    return Model.Statuses.Status.Free;
                case Yandex.Taxi.Gateway.Contracts.Status.Busy:
                    return Model.Statuses.Status.Busy;
                case Yandex.Taxi.Gateway.Contracts.Status.VeryBusy:
                    return Model.Statuses.Status.VeryBusy;
                default:
                    throw new NotSupportedException();
            }
        }

        private static HttpClient CreateHttpClient()
        {
            HttpClient result = new HttpClient();
            result.DefaultRequestHeaders.ExpectContinue = false;
            return result;
        }

        public ReadyToProceedOrderResult SendReadyToProceedOrder(string sUuid, string sOrderId, int? iCost = null)
        {
            using (HttpClient client = CreateHttpClient())
            {
                string sURI = string.Format("{0}/1.x/carack?clid={1}&apikey={2}&uuid={3}&orderid={4}", mTaxiHost, mClid, mApiKey, sUuid, sOrderId);

                if (iCost.HasValue)
                {
                    sURI += string.Format("cost={0}", iCost.Value);
                }

                mLogger.Debug("SendReadyToProceedOrder query: {0}.", sURI);

                // TODO: Reliable GET
                var result = client.GetAsync(sURI).Result;

                mLogger.Debug("SendReadyToProceedOrder query: {0}. Result: {1}", sURI, result);

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return ReadyToProceedOrderResult.Ok;
                }
                else if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return ReadyToProceedOrderResult.DriverOrOrderNotFound;
                }
                else if (result.StatusCode == System.Net.HttpStatusCode.Gone)
                {
                    return ReadyToProceedOrderResult.Assigned;
                }
                else
                {
                    return ReadyToProceedOrderResult.Unknown;
                }
            }
        }

        public OrderUpdateResult SendOrderUpdate(string sOrderId, OrderStatus eStatus, string sExtra = null, string sNewCar = null)
        {
            using (HttpClient client = CreateHttpClient())
            {
                string sURI = string.Format("{0}/1.x/requestconfirm?clid={1}&apikey={2}&orderid={3}&status={4}", mTaxiHost, mClid, mApiKey, sOrderId, Map(eStatus));

                if (!string.IsNullOrWhiteSpace(sExtra))
                {
                    sURI += string.Format("&extra={0}", sExtra);
                }
                if (!string.IsNullOrWhiteSpace(sNewCar))
                {
                    sURI += string.Format("&newcar={0}", sNewCar);
                }

                // TODO: Reliable GET
                var result = client.GetAsync(sURI).Result;

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return OrderUpdateResult.Ok;
                }
                else if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return OrderUpdateResult.DriverOrOrderNotFound;
                }
                else
                {
                    return OrderUpdateResult.Unknown;
                }
            }
        }

        private static string Map(OrderStatus eStatus)
        {
            return eStatus.ToString().ToLowerInvariant();
        }
    }
}
