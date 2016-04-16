using System;
using AutoMapper;
using Dexpa.Core;
using Dexpa.Core.Model;
using Dexpa.Core.Utils;
using Dexpa.DTO;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.WebApi.Utils
{
    public partial class ObjectMapper
    {
        private void CreateMapOrder()
        {
            Mapper.CreateMap<Order, OrderDTO>()
                .ForMember(d => d.Timestamp, opt => opt.MapFrom(d => TimeConverter.UtcToLocal(d.Timestamp)))
                .ForMember(d => d.DepartureDate, opt => opt.MapFrom(d => TimeConverter.UtcToLocal(d.DepartureDate)))
                .ForMember(d => d.TariffShortName, opt => opt.MapFrom(d => d.Tariff.Abbreviation))
                .ForMember(d => d.FromAddressDetails, opt => opt.MapFrom(d => d.FromAddress))
                .ForMember(d => d.ToAddressDetails, opt => opt.MapFrom(d => d.ToAddress));


            Mapper.CreateMap<OrderDTO, Order>()
                .ForMember(d => d.DepartureDate, opt => opt.MapFrom(d => TimeConverter.LocalToUtc(d.DepartureDate)))
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Timestamp, opt => opt.Ignore())
                .ForMember(d => d.Driver, opt => opt.Ignore())
                .ForMember(d => d.DriverId, opt => opt.MapFrom(o => o.Driver != null ? o.Driver.Id : (long?) null))
                .ForMember(d => d.Customer, opt => opt.Ignore())
                .ForMember(d => d.DepartureDate, opt => opt.MapFrom(d => TimeConverter.LocalToUtc(d.DepartureDate)))
                .ForMember(d => d.CustomerId, opt => opt.MapFrom(o => o.Customer != null ? o.Customer.Id : (long?) null))
                .ForMember(d => d.FromAddress, opt => opt.MapFrom(d => d.FromAddressDetails))
                .ForMember(d => d.ToAddress, opt => opt.MapFrom(d => d.ToAddressDetails));

            Mapper.CreateMap<OrderStateType, OrderStateDTO>()
                .ConvertUsing(OrderStateToDTO);

            Mapper.CreateMap<OrderStateDTO, OrderStateType>()
                .ConvertUsing(cs => cs == null ? OrderStateType.Created : cs.Type);

            Mapper.CreateMap<OrderOptions, OrderOptionsDTO>();
            Mapper.CreateMap<OrderOptionsDTO, OrderOptions>();

            Mapper.CreateMap<Address, AddressDTO>();
            Mapper.CreateMap<AddressDTO, Address>()
                .ForMember(d=>d.Latitude, opt=>opt.Ignore())
                .ForMember(d=>d.Longitude, opt=>opt.Ignore());

            Mapper.CreateMap<OrderSource, OrderSourceDTO>()
                .ConvertUsing(OrderSourceToDTO);

            Mapper.CreateMap<OrderSourceDTO, OrderSource>()
                .ConvertUsing(cs => cs == null ? OrderSource.Dispatcher : cs.Source);
        }

        private OrderStateDTO OrderStateToDTO(OrderStateType state)
        {
            string name;
            switch (state)
            {
                case OrderStateType.Created:
                    name = "Новый";
                    break;
                case OrderStateType.Assigned:
                    name = "Назначен";
                    break;
                case OrderStateType.Driving:
                    name = "Выехал";
                    break;
                case OrderStateType.Waiting:
                    name = "На месте";
                    break;
                case OrderStateType.Transporting:
                    name = "В пути";
                    break;
                case OrderStateType.Completed:
                    name = "Завершен";
                    break;
                case OrderStateType.Rejected:
                    name = "Отменен";
                    break;
                case OrderStateType.Failed:
                    name = "Водитель не смог выполнить заказ";
                    break;
                case OrderStateType.Canceled:
                    name = "Отменен клиентом";
                    break;
                case OrderStateType.Accepted:
                    name = "Принят";
                    break;
                case OrderStateType.Approved:
                    name = "Подтвержден";
                    break;
                    case OrderStateType.Disapproved:
                    name = "Не подтвержден";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state");
            }

            return new OrderStateDTO
            {
                Name = name,
                Type = state
            };
        }

        private OrderSourceDTO OrderSourceToDTO(OrderSource orderSource)
        {
            var name = string.Empty;
            switch (orderSource)
            {
                case OrderSource.Yandex:
                    name = "Яндекс";
                    break;
                case OrderSource.Dispatcher:
                    name = "Диспетчерская";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("orderSource");
            }

            return new OrderSourceDTO()
            {
                Name = name,
                Source = orderSource
            };
        }

    }
}