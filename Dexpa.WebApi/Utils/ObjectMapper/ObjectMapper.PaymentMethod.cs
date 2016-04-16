using System;
using AutoMapper;
using Dexpa.Core.Model;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.WebApi.Utils
{
    public partial class ObjectMapper
    {
        private void CreateMapPaymentMethod()
        {
            Mapper.CreateMap<PaymentMethod, PaymentMethodDTO>()
                .ConvertUsing(PaymentMethodToDTO);

            Mapper.CreateMap<PaymentMethodDTO, PaymentMethod>()
                .ConvertUsing(item => item.PaymentMethod);
        }

        private PaymentMethodDTO PaymentMethodToDTO(PaymentMethod method)
        {
            var name = string.Empty;
            switch (method)
            {
                case PaymentMethod.Cash:
                    name = "Наличные";
                    break;
                case PaymentMethod.NonCash:
                    name = "Безналичный расчет";
                    break;
                case PaymentMethod.Qiwi:
                    name = "QIWI";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("method");
            }

            return new PaymentMethodDTO
            {
                Name = name,
                PaymentMethod = method
            };
        }
    }
}