using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using AutoMapper;
using Dexpa.Core.Model;
using Dexpa.Core.Utils;
using Dexpa.DTO;
using Content = Dexpa.Core.Model.Content;

namespace Dexpa.WebApi.Utils
{
    public partial class ObjectMapper
    {
        private void CreateMapDriver()
        {
            Mapper.CreateMap<Driver, DriverDTO>()
                .ForMember(d => d.Phones, opt => opt.MapFrom(s => StringToPhones(s.Phones)))
                .ForMember(d => d.Callsign, opt => opt.MapFrom(s => s.Car.Callsign))
                .ForMember(d => d.Content, opt => opt.MapFrom(s => ContentsToObj(s.Content)))
                .ForMember(d => d.Timestamp, opt => opt.MapFrom(s => TimeConverter.UtcToLocal(s.Timestamp)))
                .ForMember(d => d.LastTrackUpdateTime, opt => opt.MapFrom(s => TimeConverter.UtcToLocal(s.LastTrackUpdateTime)))
                .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.Logins != null && s.Logins.Any() ? s.Logins.FirstOrDefault().UserName : null))
                .ForMember(d => d.UserPassword, opt => opt.MapFrom(s => s.Logins != null && s.Logins.Any() ? s.Logins.FirstOrDefault().DriverPassword : null));

            Mapper.CreateMap<DriverDTO, Driver>()
                .ForMember(d => d.Phones, opt => opt.MapFrom(s => PhonesToString(s.Phones)))
                .ForMember(d => d.State, opt => opt.MapFrom(s => s.State.State))
                .ForMember(d => d.Id, opt => opt.UseDestinationValue())
                .ForMember(d => d.Timestamp, opt => opt.Ignore())
                .ForMember(d => d.LastTrackUpdateTime, opt => opt.Ignore())
                .ForMember(d => d.Car, opt => opt.Ignore())
                .ForMember(d => d.CarId, opt => opt.MapFrom(d => d.Car != null ? d.Car.Id : (long?)null))
                .ForMember(d => d.WorkConditions, opt => opt.Ignore())
                .ForMember(d => d.Content, opt => opt.Ignore())
                .ForMember(d => d.WorkConditionsId, opt => opt.MapFrom(d => d.WorkConditions != null ? d.WorkConditions.Id : (long?)null));

            Mapper.CreateMap<DriverLicense, DriverLicenseDTO>();
            Mapper.CreateMap<DriverLicenseDTO, DriverLicense>();

            Mapper.CreateMap<RobotSettings, RobotSettingsDTO>();
            Mapper.CreateMap<RobotSettingsDTO, RobotSettings>();
        }

        private ContentsObjDTO ContentsToObj(ICollection<Dexpa.Core.Model.Content> contents)
        {
            if (contents == null)
            {
                return null;
            }

            var obj = new ContentsObjDTO();

            var CarBack = contents.Where(c => c.Type == DexpaContentType.Back);
            obj.CarBacks = SetContentDtosList(CarBack);

            var CarFront = contents.Where(c => c.Type == DexpaContentType.Front);
            obj.CarFronts = SetContentDtosList(CarFront);

            var DriverPhoto = contents.FirstOrDefault(c => c.Type == DexpaContentType.DriverPhoto);
            obj.DriverPhoto = DriverPhoto != null ? Map<Content, ContentDTO>(DriverPhoto) : null;

            var CarSide = contents.Where(c => c.Type == DexpaContentType.Side);
            obj.CarSides = SetContentDtosList(CarSide);

            var CarInterior = contents.Where(c => c.Type == DexpaContentType.Interior);
            obj.CarInteriors = SetContentDtosList(CarInterior);

            var CarRegNumber = contents.Where(c => c.Type == DexpaContentType.RegNumber);
            obj.CarRegNumbers = SetContentDtosList(CarRegNumber);

            return obj;
        }

        private IList<ContentDTO> SetContentDtosList(IEnumerable<Content> contents)
        {
            IList<ContentDTO> dtos = new List<ContentDTO>();
            if (contents.Any())
            {
                foreach (var content in contents)
                {
                    dtos.Add(Map<Dexpa.Core.Model.Content, ContentDTO>(content));
                }
                return dtos;
            }

            return null;
        }

        private List<long> StringToPhones(string phones)
        {
            if (phones != null)
            {
                var values = phones.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var phoneList = new List<long>(values.Length);
                for (int i = 0; i < values.Length; i++)
                {
                    phoneList.Add(long.Parse(values[i]));
                }
                return phoneList;
            }
            else
            {
                return null;
            }
        }

        private string PhonesToString(IList<long> phones)
        {
            if (phones == null)
            {
                return null;
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < phones.Count; i++)
            {
                builder.AppendFormat("{0}, ", phones[i]);
            }
            if (builder.Length > 0)
            {
                builder = builder.Remove(builder.Length - 2, 2);
            }
            return builder.ToString();
        }
    }
}