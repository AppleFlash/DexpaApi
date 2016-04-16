using System;
using System.Collections.Generic;
using AutoMapper;
using Dexpa.Core.Model;
using Dexpa.OrdersGateway.Models;
using Yandex.Taxi.Gateway.Contracts;

namespace Dexpa.YandexTaxiService
{
    public class ObjectMapper
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
            Mapper.CreateMap<Driver, YDriver>()
                .ForMember(d => d.Name, opt => opt.MapFrom(d => d.FirstName))
                .ForMember(d => d.Permit, opt => opt.MapFrom(d => FormatPermission(d.Car.Permission)))
                .ForMember(d => d.BirthYear, opt => opt.UseValue(new DateTime(1980, 1, 1)))
                .ForMember(d => d.Phone, opt => opt.MapFrom(d => d.Phones[0]));

            Mapper.CreateMap<Car, YCar>()
                .ForMember(c => c.Color, opt => opt.MapFrom(c => c.Color))
                .ForMember(c => c.Model, opt => opt.MapFrom(c => string.Format("{0} {1}", c.Brand, c.Model)))
                .ForMember(c => c.Number, opt => opt.MapFrom(c => c.RegNumber))
                .ForMember(c => c.Year, opt => opt.MapFrom(c => c.ProductionYear))
                .ForMember(c => c.Requirements, opt => opt.MapFrom(c => GetCarRequirements(c.Features, c.ChildrenSeat)));

        }

        private List<CarRequirement> GetCarRequirements(CarFeatures features, ChildrenSeat childrenSeat)
        {
            var requirements = new List<CarRequirement>();
            foreach (CarFeatures carFeature in Enum.GetValues(typeof(CarFeatures)))
            {
                if (!carFeature.HasFlag(CarFeatures.Smoke))
                {
                    requirements.Add(CarRequirement.NoSmoking);
                }
                if (carFeature.HasFlag(CarFeatures.Conditioner))
                {
                    requirements.Add(CarRequirement.HasConditioner);
                }
                if (carFeature.HasFlag(CarFeatures.WithAnimals))
                {
                    requirements.Add(CarRequirement.AnimalTransport);
                }
                if (childrenSeat != ChildrenSeat.None)
                {
                    requirements.Add(CarRequirement.ChildChair);
                }
                if (carFeature.HasFlag(CarFeatures.StationWagon))
                {
                    requirements.Add(CarRequirement.Universal);
                }
            }
            return requirements;
        }

        private string FormatPermission(CarPermission permission)
        {
            return string.Format("{0} {1} {2}", permission.Number,
                permission.Series,
                permission.Number2);
        }

        public TDestination Map<TSource, TDestination>(TSource source) where TDestination : new()
        {
            var dest = new TDestination();
            dest = Map(source, dest);
            return dest;
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination, bool skipNullFields = false)
        {
            var result = Mapper.Map(source, destination);
            return result;
        }
    }
}