using System;
using AutoMapper;
using Dexpa.Core.Model;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.WebApi.Utils
{
    public partial class ObjectMapper
    {
        private void CreatMapCarStatus()
        {
            Mapper.CreateMap<CarStatus, CarStatusDTO>()
                .ConvertUsing(CarStatusToDTO);

            Mapper.CreateMap<CarStatusDTO, CarStatus>()
                .ConvertUsing(cs => cs == null ? CarStatus.NotWorks : cs.Type);
        }

        private CarStatusDTO CarStatusToDTO(CarStatus carStatus)
        {
            var name = string.Empty;
            switch (carStatus)
            {
                case CarStatus.None:
                    name = "";
                    break;
                case CarStatus.Works:
                    name = "Работает";
                    break;
                case CarStatus.NotWorks:
                    name = "Не работает";
                    break;
                case CarStatus.Service:
                    name = "ТО";
                    break;
                case CarStatus.Repair:
                    name = "Ремонт";
                    break;
                case CarStatus.Stolen:
                    name = "Угон";
                    break;
                case CarStatus.Garage:
                    name = "В гараже";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("carStatus");
            }

            return new CarStatusDTO
            {
                Name = name,
                Type = carStatus
            };
        }
    }
}