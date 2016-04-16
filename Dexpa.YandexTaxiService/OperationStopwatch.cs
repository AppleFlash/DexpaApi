using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Dexpa.YandexTaxiService
{
    class OperationStopwatch:IDisposable
    {
        private Logger mLogger = LogManager.GetCurrentClassLogger();

        private Stopwatch mStopwatch;

        private string mMessage;

        public OperationStopwatch(string message)
        {
            mMessage = message;
            mStopwatch = new Stopwatch();
            mStopwatch.Start();
        }

        public void Dispose()
        {
            mStopwatch.Stop();
            mLogger.Debug("{0} {1} ms", mMessage, mStopwatch.ElapsedMilliseconds);
        }
    }
}
