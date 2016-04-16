using AutoMapper;
using Dexpa.Core.Model;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.WebApi.Utils
{
    public partial class ObjectMapper
    {
        private void CreateMapTariffDays()
        {
            Mapper.CreateMap<DaysEnum, DaysDTO>()
                .ConvertUsing(DaysToDTO);

            Mapper.CreateMap<DaysDTO, DaysEnum>()
                .ConvertUsing(DTOToDays);
        }

        private DaysEnum DTOToDays(DaysDTO daysDto)
        {
            var tarifDays = DaysEnum.None;
            if (daysDto == null)
            {
                return tarifDays;
            }

            if (daysDto.Monday)
            {
                tarifDays = tarifDays | DaysEnum.Monday;
            }

            if (daysDto.Tuesday)
            {
                tarifDays = tarifDays | DaysEnum.Tuesday;
            }

            if (daysDto.Wednesday)
            {
                tarifDays = tarifDays | DaysEnum.Wednesday;
            }

            if (daysDto.Thursday)
            {
                tarifDays = tarifDays | DaysEnum.Thursday;
            }

            if (daysDto.Friday)
            {
                tarifDays = tarifDays | DaysEnum.Friday;
            }

            if (daysDto.Saturday)
            {
                tarifDays = tarifDays | DaysEnum.Saturday;
            }

            if (daysDto.Sunday)
            {
                tarifDays = tarifDays | DaysEnum.Sunday;
            }

            return tarifDays;
        }

        private DaysDTO DaysToDTO(DaysEnum daysEnum)
        {
            return new DaysDTO
            {
                Monday = daysEnum.HasFlag(DaysEnum.Monday),
                Tuesday = daysEnum.HasFlag(DaysEnum.Tuesday),
                Wednesday = daysEnum.HasFlag(DaysEnum.Wednesday),
                Thursday = daysEnum.HasFlag(DaysEnum.Thursday),
                Friday = daysEnum.HasFlag(DaysEnum.Friday),
                Saturday = daysEnum.HasFlag(DaysEnum.Saturday),
                Sunday = daysEnum.HasFlag(DaysEnum.Sunday)
            };
        }
    }
}