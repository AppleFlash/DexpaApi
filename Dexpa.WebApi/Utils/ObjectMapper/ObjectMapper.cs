using System;
using AutoMapper;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Events;
using Dexpa.Core.Model.Reports;
using Dexpa.DTO;
using Dexpa.DTO.Events;

namespace Dexpa.WebApi.Utils
{
    public partial class ObjectMapper
    {
        public static ObjectMapper Instance
        {
            get
            {
                return mLazyInstance.Value;
            }
        }

        private static readonly Lazy<ObjectMapper> mLazyInstance = new Lazy<ObjectMapper>(() => new ObjectMapper());

        private ObjectMapper()
        {
            CreatMapChildrenSeat();
            CreatMapCarFeatures();
            CreatMapCarStatus();
            CreateMapCar();

            CreatMapDriverState();
            CreateMapDriver();
            CreateMapOrder();
            CreatMapOrderType();

            CreateMapPaymentMethod();
            CreateMapTransactionGroup();
            CreateMapTransactionType();
            CreateMapTransaction();
            CreateDriverWorkConditions();

            CreateMapTariff();
            CreateMapTariffDays();
            CreateMapTariffZones();

            CreateMapRegion();

            CreateMapDexpaContentType();

            CreateMapOrderHistory();

            CreateMapRepair();
            CreateMapCarEvent();

            CreateMapNewsMessage();

            Mapper.CreateMap<Customer, CustomerDTO>()
                .ForMember(d => d.Phone,
                    opt => opt.MapFrom(d => !string.IsNullOrEmpty(d.PrivatePhone)
                        ? d.PrivatePhone
                        : d.Phone));
            Mapper.CreateMap<CustomerDTO, Customer>()
                .ForMember(d => d.Id, opt => opt.UseDestinationValue())
                .ForMember(d => d.Organization, opt => opt.Ignore())
                .ForMember(d => d.OrganizationId,opt => opt.MapFrom(d => d.Organization.Id != 0 ? d.Organization.Id : (long?)null))
                .ForMember(d => d.Phone, opt => opt.MapFrom(d => d.Phone))
                .ForMember(d => d.PrivatePhone, opt => opt.MapFrom(d => d.Phone));

            Mapper.CreateMap<Location, LocationDTO>();
            Mapper.CreateMap<LocationDTO, Location>();

            Mapper.CreateMap<CarPermission, CarPermissionDTO>();
            Mapper.CreateMap<CarPermissionDTO, CarPermission>();

            Mapper.CreateMap<EventOrderStateChanged, EventOrderStateChangedDTO>()
                .ForMember(d => d.Timestamp, opt => opt.MapFrom(s => TimeHelper.UtcToLocal(s.Timestamp)));

            Mapper.CreateMap<DriversReport, DriversReportDTO>();
            Mapper.CreateMap<OrdersReport, OrdersReportDTO>();
            Mapper.CreateMap<DispatcherReport, DispatcherReportDTO>();
            Mapper.CreateMap<YandexOrdersReport, YandexOrdersReportDTO>();

            Mapper.CreateMap<SearchResult, SearchResultDTO>();

            Mapper.CreateMap<CustomerAddresses, CustomerAddressesDTO>();
            Mapper.CreateMap<CustomerAddressesDTO, CustomerAddresses>()
                .ForMember(d => d.Id, opt => opt.UseDestinationValue());

            Mapper.CreateMap<TrackPoint, TrackPointDTO>()
                .ForMember(d => d.Timestamp, opt => opt.MapFrom(s => TimeHelper.UtcToLocal(s.Timestamp)));


            Mapper.CreateMap<CustomerReportItem, CustomerReportItemDTO>();
            Mapper.CreateMap<Content, ContentDTO>();

            Mapper.CreateMap<OrderWithPriority, OrderWithPriorityDTO>();

            Mapper.CreateMap<Organization, OrganizationDTO>();
            Mapper.CreateMap<OrganizationDTO, Organization>()
                .ForMember(o => o.Id, opt => opt.UseDestinationValue())
                .ForMember(o => o.Tariff, opt => opt.Ignore())
                .ForMember(o => o.TariffId, opt => opt.MapFrom(t => t.Tariff.Id));
            Mapper.CreateMap<OrganizationOrdersReport, OrganizationOrdersReportDTO>();
            Mapper.CreateMap<IpPhoneUser, IpPhoneUserDTO>();
            Mapper.CreateMap<IpPhoneUserDTO, IpPhoneUser>();

            Mapper.CreateMap<GlobalSettings, GlobalSettingsDTO>();
            Mapper.CreateMap<GlobalSettingsDTO, GlobalSettings>();

            Mapper.CreateMap<RobotLog, RobotLogDTO>();

            Mapper.CreateMap<WayBills, WayBillsDTO>()
                .ForMember(w=>w.Timestamp, opt=>opt.MapFrom(s=>TimeHelper.UtcToLocal(s.Timestamp)));
            Mapper.CreateMap<WayBillsDTO, WayBills>()
                .ForMember(w=>w.Id, opt=>opt.UseDestinationValue())
                .ForMember(w=>w.Driver, opt=>opt.Ignore())
                .ForMember(w=>w.DriverId, opt=>opt.MapFrom(d=>d.Driver.Id))
                .ForMember(w=>w.Car, opt=>opt.Ignore())
                .ForMember(w=>w.CarId, opt=>opt.MapFrom(c=>c.Car.Id))
                .ForMember(w=>w.Timestamp, opt=>opt.Ignore());

            Mapper.CreateMap<CarEventReport, CarEventReportDTO>()
                .ForMember(d => d.Timestamp, opt => opt.MapFrom(s => TimeHelper.UtcToLocal(s.Timestamp)));
        }

        public TDestination Map<TSource, TDestination>(TSource source) where TDestination : new()
        {
            var dest = new TDestination();
            dest = Map(source, dest);
            return dest;
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination, bool skipNullFields = false)
        {
            if (source is DriverWorkConditionsDTO)
            {
                return (TDestination)MapWorkConditions(source, destination);
            }
            else
            {
                var result = Mapper.Map(source, destination);
                return result;
            }
        }
    }
}