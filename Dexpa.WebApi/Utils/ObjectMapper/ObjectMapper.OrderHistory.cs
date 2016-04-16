using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Dexpa.Core.Model;
using Dexpa.Core.Utils;
using Dexpa.DTO;

namespace Dexpa.WebApi.Utils
{
    public partial class ObjectMapper
    {
        private void CreateMapOrderHistory()
        {
            Mapper.CreateMap<OrderHistory, OrderHistoryDTO>()
                .ForMember(h => h.TimeStamp, opt => opt.MapFrom(h => TimeConverter.UtcToLocal(h.Timestamp)));
        }
    }
}