using System;
using AutoMapper;
using Dexpa.Core.Model;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.WebApi.Utils
{
    public partial class ObjectMapper
    {
        private void CreateMapTransactionGroup()
        {
            Mapper.CreateMap<TransactionGroup, TransactionGroupDTO>()
                .ConvertUsing(TransactionGroupToDTO);

            Mapper.CreateMap<TransactionGroupDTO, TransactionGroup>()
                .ConvertUsing(item => item.Group);
        }

        private TransactionGroupDTO TransactionGroupToDTO(TransactionGroup @group)
        {
            var name = string.Empty;
            switch (@group)
            {
                case TransactionGroup.Other:
                    name = "Другое";
                    break;
                case TransactionGroup.Rent:
                    name = "Арендная плата";
                    break;
                case TransactionGroup.OrderFee:
                    name = "Процент с заказа";
                    break;
                case TransactionGroup.TechSupportFee:
                    name = "Тех. поддержка";
                    break;
                case TransactionGroup.Fine:
                    name = "Штраф";
                    break;
                case TransactionGroup.Repair:
                    name = "Ремонт";
                    break;
                case TransactionGroup.QiwiError:
                    name = "Ошибка QIWI";
                    break;
                case TransactionGroup.CashDesk:
                    name = "Касса";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("group");
            }

            return new TransactionGroupDTO
            {
                Name = name,
                Group = @group
            };
        }
    }
}