using System;
using System.Collections.Generic;
using AutoMapper;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Light;
using Dexpa.Core.Utils;
using Dexpa.DTO;
using Dexpa.DTO.HelpDictionaries;
using Dexpa.DTO.Light;


namespace Dexpa.WebApi.Utils
{
    public partial class ObjectMapper
    {
        private void CreateMapTariff()
        {
            Mapper.CreateMap<Tariff, TariffDTO>()
                .ForMember(t => t.TimeFrom, opt => opt.MapFrom(s => TimeConverter.UtcToLocal(s.TimeFrom)))
                .ForMember(t => t.TimeTo, opt => opt.MapFrom(s => TimeConverter.UtcToLocal(s.TimeTo)));

            Mapper.CreateMap<TariffDTO, Tariff>()
                .ForMember(t => t.Id, opt => opt.UseDestinationValue())
                .ForMember(t => t.Timestamp, opt => opt.Ignore())
                .ForMember(t => t.TimeFrom, opt => opt.MapFrom(s => TimeConverter.LocalToUtc(s.TimeFrom)))
                .ForMember(t => t.TimeTo, opt => opt.MapFrom(s => TimeConverter.LocalToUtc(s.TimeTo)));

            Mapper.CreateMap<TariffZone, TariffZonesDTO>();
            Mapper.CreateMap<TariffZonesDTO, TariffZone>()
                .ForMember(t => t.Id, opt => opt.UseDestinationValue())
                .ForMember(t => t.TariffZoneType, opt => opt.MapFrom(t => t.TariffZoneType.Type));

            Mapper.CreateMap<TariffRegionCost, TariffRegionCostDTO>();

            Mapper.CreateMap<TariffRegionCostDTO, TariffRegionCost>()
                .ForMember(t => t.Id, opt => opt.UseDestinationValue())
                .ForMember(t => t.RegionFrom, opt => opt.Ignore())
                .ForMember(t => t.RegionTo, opt => opt.Ignore())
                .ForMember(t => t.RegionFromId, opt => opt.MapFrom(r => r.RegionFromId))
                .ForMember(t => t.RegionToId, opt => opt.MapFrom(r => r.RegionToId));

            Mapper.CreateMap<TariffOptions, TariffOptionsDTO>();
            Mapper.CreateMap<TariffOptionsDTO, TariffOptions>();

            Mapper.CreateMap<LightTariff, LightTariffDTO>()
                .ForMember(t => t.TimeFrom, opt => opt.MapFrom(s => TimeConverter.UtcToLocal(s.TimeFrom)))
                .ForMember(t => t.TimeTo, opt => opt.MapFrom(s => TimeConverter.UtcToLocal(s.TimeTo)));
        }
    }
}