using System;
using System.Linq;
using Dexpa.Core.Model;
using Dexpa.Core.Utils;
using Dexpa.SmsGateway;
using Qiwi.Parser;
using TransactionType = Dexpa.Core.Model.TransactionType;

namespace Dexpa.ServiceCore
{
    public class NewsMessagesSendingService : AService
    {
        private ISmsGateway mSmsGateway;

        public NewsMessagesSendingService()
        {
            mIterationPauseMs = 20000;

            using (var context = new OperationContext())
            {
                var globalSettings = context.GlobalSettingsService.GetSettings();
                mSmsGateway = SmsGatewayFactory.CreateSmsGateway(globalSettings.SmscLogin, globalSettings.SmscPassword);
            }
        }

        protected override void WorkIteration()
        {
            using (var context = new OperationContext())
            {
                var messages = context.NewsMessagesService.GetLastNewsMessages();
                messages = messages.Where(n => n.IsSend == false).ToList();

                if (!messages.Any())
                {
                    return;
                }

                mLogger.Debug("Sending news messages...");

                var drivers = context.DriverService.GetDrivers(false);
                var driversPhonesList = drivers.Select(d => d.Phones.Split(',').FirstOrDefault()).ToList();
                driversPhonesList = driversPhonesList.Where(p => !string.IsNullOrEmpty(p)).ToList();

                var driverPhonesString = string.Join(",", driversPhonesList);

                foreach (var newsMessage in messages)
                {
                    mSmsGateway.SendMessage(driverPhonesString, newsMessage.Message);

                    newsMessage.IsSend = true;
                    context.NewsMessagesService.UpdateNewsMessage(newsMessage);
                }
            }
        }
    }
}
