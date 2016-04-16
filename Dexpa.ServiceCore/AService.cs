using System;
using System.Threading;
using NLog;

namespace Dexpa.ServiceCore
{
    public abstract class AService
    {
        private Thread mWorkThread;

        private object mSyncObject = new object();

        private volatile bool mStopSignal;

        protected int mIterationPauseMs = 5000;

        protected Logger mLogger = LogManager.GetCurrentClassLogger();

        public void Start()
        {
            lock (mSyncObject)
            {
                if (mWorkThread == null)
                {
                    BeforeStart();
                    mWorkThread = new Thread(DoWork)
                    {
                        IsBackground = true
                    };
                    mWorkThread.Start();
                }
                else
                {
                    throw new InvalidOperationException("Service already started");
                }
            }
        }

        protected virtual void BeforeStart()
        {

        }

        protected virtual void DoWork()
        {
            while (!mStopSignal)
            {
                try
                {
                    WorkIteration();
                }
                catch (ThreadAbortException)
                {
                    //Do nothing
                }
                catch (Exception exception)
                {
                    mLogger.Error("WorkIterationError", exception);
                }
                Thread.Sleep(mIterationPauseMs);
            }
        }

        protected abstract void WorkIteration();

        public void Stop()
        {
            lock (mSyncObject)
            {
                if (mWorkThread != null)
                {
                    mStopSignal = true;
                    mWorkThread.Join();
                    mStopSignal = false;
                    mWorkThread = null;
                }
            }
        }
    }
}
