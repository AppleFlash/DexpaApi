using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dexpa.Core.Services;
using Dexpa.Infrastructure;
using Dexpa.Ioc;

namespace Dexpa.YandexTaxiService
{
    class OperationContext : IDisposable
    {
        public IOrderService OrderService
        {
            get
            {
                if (mOrderService == null)
                {
                    mOrderService = IocFactory.Instance.Create<IOrderService>(mScope);
                }
                return mOrderService;
            }
        }

        public IDriverService DriverService
        {
            get
            {
                mDriverService = IocFactory.Instance.Create<IDriverService>(mScope);
                return mDriverService;
            }
        }

        public ITariffsService TariffsService
        {
            get
            {
                mTariffsService = IocFactory.Instance.Create<ITariffsService>(mScope);
                return mTariffsService;
            }
        }

        public IEventService EventService
        {
            get
            {
                if (mEventService == null)
                {
                    mEventService = IocFactory.Instance.Create<IEventService>(mScope);
                }
                return mEventService;
            }
        }

        public IOrderRequestService OrderRequestService
        {
            get
            {
                if (mOrderRequestService == null)
                {
                    mOrderRequestService = IocFactory.Instance.Create<IOrderRequestService>(mScope);
                }
                return mOrderRequestService;
            }
        }

        public IRobotLogService RobotLogService
        {
            get
            {
                if (mRobotLogService == null)
                {
                    mRobotLogService = IocFactory.Instance.Create<IRobotLogService>(mScope);
                }
                return mRobotLogService;
            }
        }

        public IDriverOrderRequestService DriverOrderRequestService
        {
            get
            {
                if (mDriverOrderRequestService == null)
                {
                    mDriverOrderRequestService = IocFactory.Instance.Create<DriverOrderRequestService>(mScope);
                }
                return mDriverOrderRequestService;
            }
        }

        public INewsMessagesService NewsMessagesService
        {
            get
            {
                if (mNewsMessagesService == null)
                {
                    mNewsMessagesService = IocFactory.Instance.Create<NewsMessagesService>(mScope);
                }
                return mNewsMessagesService;
            }
        }

        private IDbTransaction mTransaction;

        private DbContext mDbContext;

        private object mScope;
        private IOrderRequestService mOrderRequestService;
        private IEventService mEventService;
        private ITariffsService mTariffsService;
        private IDriverService mDriverService;
        private IOrderService mOrderService;
        private IDriverOrderRequestService mDriverOrderRequestService;
        private IRobotLogService mRobotLogService;
        private INewsMessagesService mNewsMessagesService;

        public OperationContext()
        {
            mScope = new object();
            mDbContext = IocFactory.Instance.Create<DbContext>(mScope);
            // mTransaction = transactionFactory.BeginTransaction();
        }

        public void Dispose()
        {
            //IocFactory.Instance.Release(mDbContext);
            mDbContext.Dispose();
            //  mTransaction.Commit();
        }
    }
}
