using System;
using AutoMapper;
using Dexpa.Core.Model;
using Dexpa.Core.Utils;
using Dexpa.DTO;

namespace Dexpa.WebApi.Utils
{
    public partial class ObjectMapper
    {
        private void CreateMapNewsMessage()
        {
            Mapper.CreateMap<NewsMessage, NewsMessageDTO>()
                .ForMember(c => c.TimeStamp, opt => opt.MapFrom(c => TimeConverter.UtcToLocal(c.TimeStamp)));

            Mapper.CreateMap<NewsMessageDTO, NewsMessage>()
                .ForMember(c => c.Id, opt => opt.UseDestinationValue())
                .ForMember(c => c.TimeStamp, opt => opt.Ignore());
        }
    }
}