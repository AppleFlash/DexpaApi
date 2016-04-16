using System.Configuration;
using System.Data.Entity;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;
using Dexpa.Core.Services;
using Dexpa.Core.Utils;
using Dexpa.Infrastructure;
using Dexpa.Infrastructure.Repositories;
using Dexpa.Infrastructure.Services;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject.Modules;
using Ninject.Web.Common;

namespace Dexpa.Ioc
{
    class CoreModule : NinjectModule
    {
        public override void Load()
        {
            var regionStorageFilename = ConfigurationManager.AppSettings["RegionStorage"];

            Bind<IdentityDbContext<User>>().To<ModelContext>().InScope(s => IocFactory.Instance.Scope);
            Bind<TransactionsFactory>().ToSelf().InScope(s => IocFactory.Instance.Scope);
            Bind<RegionBinaryStorage>().ToConstant(new RegionBinaryStorage(regionStorageFilename));

            //Repositories
            Bind<DbContext>().To<ModelContext>().InScope(s => IocFactory.Instance.Scope);
            Bind<ICustomerRepository>().To<CustomerRepository>().InScope(s => IocFactory.Instance.Scope);
            Bind<IDriverRepository>().To<DriverRepository>().InScope(s => IocFactory.Instance.Scope);
            Bind<IOrderRepository>().To<OrderRepository>().InScope(s => IocFactory.Instance.Scope);
            Bind<ICarRepository>().To<CarRepository>().InScope(s => IocFactory.Instance.Scope);
            Bind<ITransactionRepository>().To<TransactionRepository>().InScope(s => IocFactory.Instance.Scope);
            Bind<IEventRepository>().To<EventRepository>().InScope(s => IocFactory.Instance.Scope);
            Bind<IDriverWorkConditionsRepository>().To<DriverWorkConditionsRepository>().InScope(s => IocFactory.Instance.Scope);
            Bind<ITariffRepository>().To<TariffRepository>().InScope(s => IocFactory.Instance.Scope);
            Bind<IOrderHistoryRepository>().To<OrderHistoryRepository>().InScope(s => IocFactory.Instance.Scope);
            Bind<IRegionRepository>().To<RegionRepository>().InScope(s => IocFactory.Instance.Scope);
            Bind<ICustomerAddressesRepository>().To<CustomerAddressesRepository>().InScope(s => IocFactory.Instance.Scope);
            Bind<ITrackPointRepository>().To<TrackPointRepository>().InScope(s => IocFactory.Instance.Scope);
            Bind<IContentRepository>().To<ContentRepository>().InScope(s => IocFactory.Instance.Scope);
            Bind<IOrderRequestRepository>().To<OrderRequestRepository>().InScope(s => IocFactory.Instance.Scope);
            Bind<IDriverOrderRequestRepository>().To<DriverOrderRequestRepository>().InScope(s => IocFactory.Instance.Scope);
            Bind<IGlobalSettingsRepository>().To<GlobalSettingsRepository>().InScope(s => IocFactory.Instance.Scope);
            Bind<IOrganizationRepository>().To<OrganizationRepository>().InScope(s => IocFactory.Instance.Scope);
            Bind<IRobotLogRepository>().To<RobotLogRepository>().InScope(s => IocFactory.Instance.Scope);
            Bind<IWayBillsRepository>().To<WayBillsrepository>().InRequestScope();
            Bind<IRepairRepository>().To<RepairRepository>().InRequestScope();
            Bind<ICarEventRepository>().To<CarEventRepository>().InRequestScope();
            Bind<INewsMessagesRepository>().To<NewsMessagesRepository>().InRequestScope();
            Bind<IDriverScoresRepository>().To<DriverScoresRepository>().InRequestScope();
            Bind<ICustomerFeedbackRepository>().To<CustomerFeedbackRepository>().InRequestScope();


            //Services
            Bind<IDriverService>().To<DriverService>().InScope(s => IocFactory.Instance.Scope);
            Bind<ICustomerService>().To<CustomerService>().InScope(s => IocFactory.Instance.Scope);
            Bind<IOrderService>().To<OrderService>().InScope(s => IocFactory.Instance.Scope);
            Bind<IOrderHistoryService>().To<OrderHistoryService>().InScope(s => IocFactory.Instance.Scope);
            Bind<ICarService>().To<CarService>().InScope(s => IocFactory.Instance.Scope);
            Bind<ITransactionService>().To<TransactionService>().InScope(s => IocFactory.Instance.Scope);
            Bind<IEventService>().To<EventService>().InScope(s => IocFactory.Instance.Scope);
            Bind<IDriverWorkConditionsService>().To<DriverWorkConditionsService>().InScope(s => IocFactory.Instance.Scope);
            Bind<IReportService>().To<ReportService>().InScope(s => IocFactory.Instance.Scope);
            Bind<ITariffsService>().To<TariffsService>().InScope(s => IocFactory.Instance.Scope);
            Bind<IAdvancedSearchService>().To<AdvancedSearchService>().InScope(s => IocFactory.Instance.Scope);
            Bind<IGeocoderService>().To<GeocoderService>().InScope(s => IocFactory.Instance.Scope);
            Bind<IRegionService>().To<RegionService>().InScope(s => IocFactory.Instance.Scope);
            Bind<ICustomerAddressesService>().To<CustomerAddressesService>().InScope(s => IocFactory.Instance.Scope);
            Bind<ITrackPointService>().To<TrackPointService>().InScope(s => IocFactory.Instance.Scope);
            Bind<IContentService>().To<ContentService>().InScope(s => IocFactory.Instance.Scope);
            Bind<IAccountService>().To<AccountService>().InScope(s => IocFactory.Instance.Scope);
            Bind<IOrderRequestService>().To<OrderRequestService>().InScope(s => IocFactory.Instance.Scope);
            Bind<IDriverOrderRequestService>().To<DriverOrderRequestService>().InScope(s => IocFactory.Instance.Scope);
            Bind<IGlobalSettingsService>().To<GlobalSettingsService>().InScope(s => IocFactory.Instance.Scope);
            Bind<IOrganizationService>().To<OrganizationService>().InScope(s => IocFactory.Instance.Scope);
            Bind<IRobotLogService>().To<RobotLogService>().InScope(s => IocFactory.Instance.Scope);
            Bind<IRepairService>().To<RepairService>().InRequestScope();
            Bind<ICarEventService>().To<CarEventService>().InRequestScope();
            Bind<ICarEventReportService>().To<CarEventReportService>().InRequestScope();
            Bind<INewsMessagesService>().To<NewsMessagesService>().InRequestScope();
        }
    }
}
