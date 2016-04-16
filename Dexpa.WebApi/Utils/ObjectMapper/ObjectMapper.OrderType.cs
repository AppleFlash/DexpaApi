using System;
using AutoMapper;
using Dexpa.Core.Model;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.WebApi.Utils
{
    public partial class ObjectMapper
    {
        private void CreatMapOrderType()
        {
            Mapper.CreateMap<OrderType, OrderTypeDTO>()
                .ConvertUsing(OrderTypeToDTO);

            Mapper.CreateMap<OrderTypeDTO, OrderType>()
                .ConvertUsing(cs => cs == null ? OrderType.None : cs.Type);
        }

        private OrderTypeDTO OrderTypeToDTO(OrderType orderType)
        {
            var name = string.Empty;
            switch (orderType)
            {
                case OrderType.None:
                    name = "Обычный";
                    break;
                case OrderType.OnlineOrder:
                    name = "Интернет-заказ";
                    break;
                case OrderType.NonCash:
                    name = "Безналичный";
                    break;
                case OrderType.OnStreet:
                    name = "От борта";
                    break;
                case OrderType.Airport:
                    name = "Аэропорт";
                    break;
                case OrderType.Yandex:
                    name = "Яндекс";
                    break;
                case OrderType.Preliminary:
                    name = "Предварительный";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("orderType");
            }

            return new OrderTypeDTO
            {
                Name = name,
                Type = orderType
            };
        }
    }
}