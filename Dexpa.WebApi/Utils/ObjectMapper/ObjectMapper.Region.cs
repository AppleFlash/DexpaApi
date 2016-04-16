using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Dexpa.Core.Model;
using Dexpa.DTO;

namespace Dexpa.WebApi.Utils
{
    public partial class ObjectMapper
    {
        private void CreateMapRegion()
        {
            Mapper.CreateMap<RegionPoint, RegionPointDTO>();
            Mapper.CreateMap<RegionPointDTO, RegionPoint>()
                .ForMember(d => d.Id, opt => opt.UseDestinationValue());

            Mapper.CreateMap<Region, RegionDTO>();
            Mapper.CreateMap<RegionDTO, Region>()
                .ForMember(d => d.Id, opt => opt.UseDestinationValue());
        }
    }
}