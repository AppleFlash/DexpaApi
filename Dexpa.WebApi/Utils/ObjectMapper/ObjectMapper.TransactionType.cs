using System;
using AutoMapper;
using Dexpa.Core.Model;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.WebApi.Utils
{
    public partial class ObjectMapper
    {
        private void CreateMapTransactionType()
        {
            Mapper.CreateMap<TransactionType, TransactionTypeDTO>()
                .ConvertUsing(TransactionTypeToDTO);

            Mapper.CreateMap<TransactionTypeDTO, TransactionType>()
                .ConvertUsing(item => item.Type);
        }

        private TransactionTypeDTO TransactionTypeToDTO(TransactionType transactionType)
        {
            var name = string.Empty;
            switch (transactionType)
            {
                case TransactionType.Replenishment:
                    name = "Пополнение";
                    break;
                case TransactionType.Withdrawal:
                    name = "Списание";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("transactionType");
            }

            return new TransactionTypeDTO
            {
                Name = name,
                Type = transactionType
            };
        }
    }
}