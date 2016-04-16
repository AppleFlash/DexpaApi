using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dexpa.Core.Model;
using Dexpa.YandexCabinet.Parser;
using CustomerFeedback = Dexpa.Core.Model.CustomerFeedback;
using DriverScores = Dexpa.Core.Model.DriverScores;

namespace Dexpa.ServiceCore
{
    public class YandexCabinetParserService : AService
    {

        private string mUrl;

        public YandexCabinetParserService(string url)
        {
            mIterationPauseMs = 60*60*1000;
            mUrl = url;
        }

        protected override void WorkIteration()
        {
            using (var context = new OperationContext())
            {
                var globalSettings = context.GlobalSettingsService.GetSettings();

                var login = globalSettings.YandexCabLogin;
                var password = globalSettings.YandexCabPassword;
                var cabinetId = globalSettings.YandexCabId;

                var yandexParser = new YandexCabinetParser(login, password, cabinetId, mUrl);

                mLogger.Debug("Getting drivers rating...");
                var driverScores = yandexParser.GetRatings();
                mLogger.Debug("Got {0} rating records.", driverScores.Count);

                mLogger.Debug("Getting orders feedbcks...");
                var feedbacks = yandexParser.GetFeedback();
                mLogger.Debug("Got {0} feedbacks", feedbacks.Count);

                List<Dexpa.Core.Model.DriverScores> updateDriverScores = new List<DriverScores>();

                foreach (var driverScore in driverScores)
                {
                    DriverScores updateScore = new DriverScores()
                    {
                        DriverId = driverScore.DriverId,
                        AverageClientScore = driverScore.AverageClientScore,
                        CancelledOrders = driverScore.CancelledOrders,
                        ClientFeedbacksCount = driverScore.ClientFeedbacksCount,
                        DriverLateScore = driverScore.DriverLateScore,
                        ExamDate = driverScore.ExamDate,
                        ExamResult = driverScore.ExamResult,
                        FakeWaitings = driverScore.FakeWaitings,
                        Total = driverScore.Total,
                        TrackQuality = driverScore.TrackQuality
                    };
                    updateDriverScores.Add(updateScore);
                }

                context.DriverService.UpdateDriverRating(updateDriverScores);
                mLogger.Debug("Added rating in database...");

                List<CustomerFeedback> updateCustomerFeedbacks = new List<CustomerFeedback>();

                List<string> sourceOrderIds = feedbacks.Select(o => o.SourceOrderId).ToList();

                var orders = context.OrderService.GetFeedbackOrders(sourceOrderIds);

                foreach (var feedback in feedbacks)
                {
                    var orderId = orders.Single(o => o.SourceOrderId == feedback.SourceOrderId).Id;
                    var driverId = orders.Single(o => o.SourceOrderId == feedback.SourceOrderId).DriverId;
                    var customer = orders.Single(o => o.SourceOrderId == feedback.SourceOrderId).Customer;
                    var date = orders.Single(o => o.SourceOrderId == feedback.SourceOrderId).DepartureDate;

                    CustomerFeedback updateFeedback = new CustomerFeedback()
                    {
                        OrderId = orderId,
                        DriverId = (long)driverId,
                        Customer = customer,
                        CustomerId = customer.Id,
                        Date = date,
                        Comment = feedback.Comment,
                        Score = feedback.Score
                    };
                    updateCustomerFeedbacks.Add(updateFeedback);
                }

                context.DriverService.UpdateCustomerFeedbacks(updateCustomerFeedbacks);
                mLogger.Debug("Added customer feedbacks...");
            }
        }
    }
}
