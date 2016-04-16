using AutoMapper;
using Dexpa.Core.Model;
using Dexpa.Core.Utils;
using Dexpa.DTO;

namespace Dexpa.WebApi.Utils
{
    public partial class ObjectMapper
    {
        private void CreateMapTransaction()
        {
            Mapper.CreateMap<Transaction, TransactionDTO>()
                .ForMember(t => t.Timestamp, opt => opt.MapFrom(t => TimeConverter.UtcToLocal(t.Timestamp)));

            Mapper.CreateMap<TransactionDTO, Transaction>()
                .ForMember(t => t.Id, opt => opt.Ignore())
                .ForMember(t => t.Timestamp, opt => opt.Ignore())
                .ForMember(t => t.PaymentMethod, opt => opt.MapFrom(t => t.PaymentMethod.PaymentMethod))
                .ForMember(t => t.Type, opt => opt.MapFrom(t => t.Type.Type))
                .ForMember(t => t.Driver, opt => opt.Ignore())
                .ForMember(t => t.DriverId, opt => opt.MapFrom(d => d.Driver.Id))
                .ForMember(t=>t.Order, opt=>opt.Ignore())
                .ForMember(t=>t.OrderId,opt=>opt.MapFrom(o=>o.Order.Id));
        }
    }
}