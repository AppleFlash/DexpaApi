using System;
using AutoMapper;
using Dexpa.Core.Model;
using Dexpa.Core.Utils;
using Dexpa.DTO;

namespace Dexpa.WebApi.Utils
{
    public partial class ObjectMapper
    {
        private void CreateMapCar()
        {
            string carBrandLogosPath = "/Content/BrandLogos/";
            Mapper.CreateMap<Car, CarDTO>()
                .ForMember(c => c.CarClass, opt => opt.MapFrom(c => c.CarClass.ToString()))
                .ForMember(c => c.BrandLogo, opt => opt.MapFrom(c => carBrandLogosPath + c.Brand.ToLower() + ".png"))
                .ForMember(c => c.Features, opt => opt.MapFrom(c => c.Features))
                .ForMember(c => c.Status, opt => opt.MapFrom(c => c.Status))
                .ForMember(c => c.ChildrenSeat, opt => opt.MapFrom(c => c.ChildrenSeat))
                .ForMember(c => c.Timestamp, opt => opt.MapFrom(c => TimeConverter.UtcToLocal(c.Timestamp)));

            Mapper.CreateMap<CarDTO, Car>()
                .ForMember(c => c.Id, opt => opt.UseDestinationValue())
                .ForMember(c => c.CarClass, opt => opt.MapFrom(c => c.CarClass == null ? CarClass.A : Enum.Parse(typeof(CarClass), c.CarClass, true)))
                .ForMember(c => c.Features, opt => opt.MapFrom(c => c.Features))
                .ForMember(c => c.Status, opt => opt.MapFrom(c => c.Status.Type))
                .ForMember(c => c.ChildrenSeat, opt => opt.MapFrom(c => c.ChildrenSeat.Type))
                .ForMember(c => c.Timestamp, opt => opt.Ignore());
        }
    }
}