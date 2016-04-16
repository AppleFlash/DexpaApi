using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexpa.ServiceCore
{
    public class BalanceRecalculateWorker : AService
    {
        private TimeSpan mRecalculateInterval;

        public BalanceRecalculateWorker()
        {
            var settings = ConfigurationManager.AppSettings;
            mRecalculateInterval = TimeSpan.Parse(settings["BalanceRecalculateTimeInterval"],
                CultureInfo.InvariantCulture);

            mIterationPauseMs = (int)mRecalculateInterval.TotalSeconds;
        }
        protected override void WorkIteration()
        {
            using (var context = new OperationContext())
            {
                context.TransactionService.RecalculateDriverBalance();
            }
        }
    }
}
