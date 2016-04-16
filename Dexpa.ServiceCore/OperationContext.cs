using System;
using System.Data;
using System.Data.Entity;
using Dexpa.Core.Repositories;
using Dexpa.Core.Services;
using Dexpa.Ioc;

namespace Dexpa.ServiceCore
{
    public class OperationContext : IDisposable
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

        public ITransactionService TransactionService
        {
            get
            {
                if (mTransactionService == null)
                {
                    mTransactionService = IocFactory.Instance.Create<ITransactionService>(mScope);
                }
                return mTransactionService;
            }
        }

        public IGlobalSettingsService GlobalSettingsService
        {
            get
            {
                if (mGlobalSettingsService == null)
                {
                    mGlobalSettingsService = IocFactory.Instance.Create<IGlobalSettingsService>(mScope);
                }
                return mGlobalSettingsService;
            }
        }

        public IAccountService AccountService
        {
            get
            {
                if (mAccountService == null)
                {
                    mAccountService = IocFactory.Instance.Create<IAccountService>(mScope);
                }
                return mAccountService;
            }
        }

        public INewsMessagesService NewsMessagesService
        {
            get
            {
                if (mNewsMessagesService == null)
                {
                    mNewsMessagesService = IocFactory.Instance.Create<INewsMessagesService>(mScope);
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
        private ITransactionService mTransactionService;
        private IGlobalSettingsService mGlobalSettingsService;
        private IAccountService mAccountService;
        private INewsMessagesService mNewsMessagesService;

        public OperationContext()
        {
            mScope = new object();
            mDbContext = IocFactory.Instance.Create<DbContext>(mScope);
            // mTransaction = transactionFactory.BeginTransaction();
        }

        public void Dispose()
        {
            mDbContext.Dispose();
            //  mTransaction.Commit();
        }
    }
}
