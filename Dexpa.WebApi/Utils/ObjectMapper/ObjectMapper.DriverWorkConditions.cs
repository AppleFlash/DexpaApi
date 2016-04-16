using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Dexpa.Core.Model;
using Dexpa.DTO;
using Dexpa.DTO.Light;

namespace Dexpa.WebApi.Utils
{
    public partial class ObjectMapper
    {
        private void CreateDriverWorkConditions()
        {
            Mapper.CreateMap<OrderFee, OrderFeeDTO>();
            Mapper.CreateMap<OrderFeeDTO, OrderFee>()
                .ForMember(d => d.Id, opt => opt.UseDestinationValue())
                .ForMember(d => d.OrderType, opt => opt.MapFrom(d => d.OrderType.Type));

            Mapper.CreateMap<DriverWorkConditions, DriverWorkConditionsDTO>();
            Mapper.CreateMap<DriverWorkConditionsDTO, DriverWorkConditions>()
                .ForMember(d => d.Id, opt => opt.UseDestinationValue());

            Mapper.CreateMap<DriverWorkConditions, WorkConditionLightDTO>()
                .ForMember(d => d.Id, opt => opt.MapFrom(d => d.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(d => d.Name));
        }

        private object MapWorkConditions(object source, object destination)
        {
            var destWorkConditions = (DriverWorkConditions)destination;
            var srcWorkConditions = (DriverWorkConditionsDTO)source;

            var sourceFees = srcWorkConditions.OrderFees;
            var destinationFees = destWorkConditions.OrderFees;

            var resultFees = new List<OrderFee>();
            foreach (var orderFeeDto in sourceFees)
            {
                var destFee = destinationFees.FirstOrDefault(d => d.Id == orderFeeDto.Id);
                if (destFee != null)
                {
                    destFee = Map(orderFeeDto, destFee);
                }
                else
                {
                    destFee = Map<OrderFeeDTO, OrderFee>(orderFeeDto);
                }
                resultFees.Add(destFee);
            }

            destWorkConditions.OrderFees = resultFees;
            destWorkConditions.Name = srcWorkConditions.Name;
            return destination;
        }
    }
}