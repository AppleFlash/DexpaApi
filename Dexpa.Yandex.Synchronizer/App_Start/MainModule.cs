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

namespace Yandex.Synchronizer
{
    public class MainModule : NinjectModule
    {
        public override void Load()
        {
            var regionStorageFilename = ConfigurationManager.AppSettings["RegionStorage"];
            Bind<IdentityDbContext<User>>().To<ModelContext>();
            Bind<RegionBinaryStorage>().ToConstant(new RegionBinaryStorage(regionStorageFilename));
            Bind<IdentityDbContext<User>>().To<ModelContext>();

            //Repositories
            Bind<DbContext>().To<ModelContext>().InRequestScope();
            Bind<ICustomerRepository>().To<CustomerRepository>().InRequestScope();
            Bind<IDriverRepository>().To<DriverRepository>().InRequestScope();
            Bind<IOrderRepository>().To<OrderRepository>().InRequestScope();
            Bind<ICarRepository>().To<CarRepository>().InRequestScope();
            Bind<ITransactionRepository>().To<TransactionRepository>().InRequestScope();
            Bind<IEventRepository>().To<EventRepository>().InRequestScope();
            Bind<IDriverWorkConditionsRepository>().To<DriverWorkConditionsRepository>().InRequestScope();
            Bind<ITariffRepository>().To<TariffRepository>().InRequestScope();
            Bind<IOrderHistoryRepository>().To<OrderHistoryRepository>().InRequestScope();
            Bind<IRegionRepository>().To<RegionRepository>().InRequestScope();
            Bind<ICustomerAddressesRepository>().To<CustomerAddressesRepository>().InRequestScope();
            Bind<ITrackPointRepository>().To<TrackPointRepository>().InRequestScope();
            Bind<IContentRepository>().To<ContentRepository>().InRequestScope();
            Bind<IOrderRequestRepository>().To<OrderRequestRepository>().InRequestScope();
            Bind<IDriverOrderRequestRepository>().To<DriverOrderRequestRepository>().InRequestScope();
            Bind<IOrganizationRepository>().To<OrganizationRepository>().InRequestScope();
            Bind<IGlobalSettingsRepository>().To<GlobalSettingsRepository>().InRequestScope();
            Bind<IRobotLogRepository>().To<RobotLogRepository>().InRequestScope();
            Bind<IWayBillsRepository>().To<WayBillsrepository>().InRequestScope();
            Bind<IRepairRepository>().To<RepairRepository>().InRequestScope();
            Bind<ICarEventRepository>().To<CarEventRepository>().InRequestScope();

            //Services
            Bind<IDriverService>().To<DriverService>().InRequestScope();
            Bind<ICustomerService>().To<CustomerService>().InRequestScope();
            Bind<IOrderService>().To<OrderService>().InRequestScope();
            Bind<ICarService>().To<CarService>().InRequestScope();
            Bind<ITransactionService>().To<TransactionService>().InRequestScope();
            Bind<IEventService>().To<EventService>().InRequestScope();
            Bind<IDriverWorkConditionsService>().To<DriverWorkConditionsService>().InRequestScope();
            Bind<IReportService>().To<ReportService>().InRequestScope();
            Bind<ITariffsService>().To<TariffsService>().InRequestScope();
            Bind<IAdvancedSearchService>().To<AdvancedSearchService>().InRequestScope();
            Bind<IGeocoderService>().To<GeocoderService>().InRequestScope();
            Bind<IRegionService>().To<RegionService>().InRequestScope();
            Bind<ICustomerAddressesService>().To<CustomerAddressesService>().InRequestScope();
            Bind<ITrackPointService>().To<TrackPointService>().InRequestScope();
            Bind<IContentService>().To<ContentService>().InRequestScope();
            Bind<IAccountService>().To<AccountService>().InRequestScope();
            Bind<IOrderRequestService>().To<OrderRequestService>().InRequestScope();
            Bind<IDriverOrderRequestService>().To<DriverOrderRequestService>().InRequestScope();
            Bind<IOrganizationService>().To<OrganizationService>().InRequestScope();
            Bind<IGlobalSettingsService>().To<GlobalSettingsService>().InRequestScope();
            Bind<IRobotLogService>().To<RobotLogService>().InRequestScope();
            Bind<IRepairService>().To<RepairService>().InRequestScope();
            Bind<ICarEventService>().To<CarEventService>().InRequestScope();
            Bind<ICarEventReportService>().To<CarEventReportService>().InRequestScope();
        }
    }
}
