using System;
using AutoMapper;
using Dexpa.Core.Model;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.WebApi.Utils
{
    public partial class ObjectMapper
    {
        private void CreateMapTariffZones()
        {
            Mapper.CreateMap<TariffZoneType, TariffZonesTypeDTO>()
                .ConvertUsing(DtoToTarifZone);
        }

        private TariffZonesTypeDTO DtoToTarifZone(TariffZoneType zoneType)
        {
            var name = string.Empty;
            switch (zoneType)
            {
                case TariffZoneType.City:
                    name = "Город";
                    break;
                case TariffZoneType.OutCity:
                    name = "За город";
                    break;
                case TariffZoneType.MKAD:
                    name = "МКАД";
                    break;
                case TariffZoneType.OutMKAD:
                    name = "За МКАД";
                    break;
                case TariffZoneType.Service:
                    name = "Подача";
                    break;
                case TariffZoneType.Everywhere:
                    name = "Везде";
                    break;
                case TariffZoneType.ServiceOutCity:
                    name = "Подача за город";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("zoneType");
            }

            return new TariffZonesTypeDTO
            {
                Name = name,
                Type = zoneType
            };
        }
    }
}