using System;
using System.Linq;
using AutoMapper;
using Dexpa.Core.Model;
using Dexpa.Core.Utils;
using Dexpa.DTO;

namespace Dexpa.WebApi.Utils
{
    public partial class ObjectMapper
    {
        private void CreateMapRepair()
        {
            Mapper.CreateMap<Repair, RepairDTO>()
                .ForMember(d => d.Timestamp, opt => opt.MapFrom(s => TimeConverter.UtcToLocal(s.Timestamp)))
                .ForMember(d => d.ImplementedByName, opt => opt.MapFrom(s => GetUserName(s.ImplementedBy)))
                .ForMember(r=>r.ImplementedByLogin, opt=>opt.MapFrom(u=>u.ImplementedBy.Logins.FirstOrDefault()));

            Mapper.CreateMap<RepairDTO, Repair>()
                .ForMember(r => r.Timestamp, opt => opt.Ignore())
                .ForMember(r => r.DamagesPhotos, opt => opt.Ignore())
                .ForMember(r=>r.ImplementedBy,opt=>opt.Ignore());
        }

        private void CreateMapCarEvent()
        {
            Mapper.CreateMap<CarEvent, CarEventDTO>()
                .ForMember(d => d.Timestamp, opt => opt.MapFrom(s => TimeConverter.UtcToLocal(s.Timestamp)))
                .ForMember(d => d.ImplementedByName, opt => opt.MapFrom(s => GetUserName(s.ImplementedBy)))
                .ForMember(r => r.ImplementedByLogin, opt => opt.MapFrom(u => u.ImplementedBy.Logins.FirstOrDefault()));

            Mapper.CreateMap<CarEventDTO, CarEvent>()
                .ForMember(r => r.Timestamp, opt => opt.Ignore())
                .ForMember(r => r.ImplementedBy, opt => opt.Ignore());
        }

        private string GetUserName(User user)
        {
            var name = "";
            if(user != null)
                name  = user.LastName ?? "" + " " + user.Name ?? "" + " " + user.MiddleName ?? "";

            return name;
        }
    }
}