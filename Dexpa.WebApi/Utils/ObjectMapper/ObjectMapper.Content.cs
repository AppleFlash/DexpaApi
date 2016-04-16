using System;
using AutoMapper;
using Dexpa.Core.Model;
using Dexpa.Core.Utils;
using Dexpa.DTO;

namespace Dexpa.WebApi.Utils
{
    public partial class ObjectMapper
    {
        private void CreateMapContent()
        {
            Mapper.CreateMap<Content, ContentDTO>()
                .ForMember(c => c.WebUrl, opt => opt.MapFrom(c => c.WebUrl))
                .ForMember(c => c.WebUrlSmall, opt => opt.MapFrom(c => c.WebUrlSmall))
                .ForMember(c => c.Id, opt => opt.MapFrom(c => c.Id))
                .ForMember(c => c.Type, opt => opt.MapFrom(c => c.Type))
                .ForMember(d => d.TimeStamp, opt => opt.MapFrom(s => TimeConverter.UtcToLocal(s.TimeStamp)));

            Mapper.CreateMap<ContentDTO, Content>()
                .ForMember(c => c.Id, opt => opt.UseDestinationValue())
                .ForMember(c => c.WebUrl, opt => opt.MapFrom(c => c.WebUrl))
                .ForMember(c => c.WebUrlSmall, opt => opt.MapFrom(c => c.WebUrlSmall))
                .ForMember(c => c.Type, opt => opt.MapFrom(c => c.Type.Type))
                .ForMember(d => d.TimeStamp, opt => opt.Ignore());
        }
    }
}