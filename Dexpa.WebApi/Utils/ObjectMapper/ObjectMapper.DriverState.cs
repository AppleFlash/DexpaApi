using System;
using AutoMapper;
using Dexpa.Core.Model;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.WebApi.Utils
{
    public partial class ObjectMapper
    {
        private void CreatMapDriverState()
        {
            Mapper.CreateMap<DriverState, DriverStateDTO>()
                .ConvertUsing(DriverStatesToDTO);

            Mapper.CreateMap<DriverStateDTO, DriverState>()
                .ConvertUsing(cs => cs == null ? DriverState.Fired : cs.State);
        }

        private DriverStateDTO DriverStatesToDTO(DriverState driverState)
        {
            var name = string.Empty;
            switch (driverState)
            {
                case DriverState.ReadyToWork:
                    name = "Свободен";
                    break;
                case DriverState.NotAvailable:
                    name = "Недоступен";
                    break;
                case DriverState.Busy:
                    name = "На заказе";
                    break;
                case DriverState.Fired:
                    name = "Уволен";
                    break;
                case DriverState.Blocked:
                    name = "Заблокирован";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("driverState");
            }

            return new DriverStateDTO
            {
                Name = name,
                State = driverState
            };
        }
    }
}