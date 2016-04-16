using System;
using AutoMapper;
using Dexpa.Core.Model;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.WebApi.Utils
{
    public partial class ObjectMapper
    {
        private void CreateMapDexpaContentType()
        {
            Mapper.CreateMap<DexpaContentType, DexpaContentTypeDTO>()
                .ConvertUsing(DexpaContentTypeToDTO);

            Mapper.CreateMap<DexpaContentTypeDTO, DexpaContentType>()
                .ConvertUsing(cs => cs.Type);
        }

        private DexpaContentTypeDTO DexpaContentTypeToDTO(DexpaContentType contentType)
        {
            var name = string.Empty;
            switch (contentType)
            {
                case DexpaContentType.DriverPhoto:
                    name = "Фотография водителя";
                    break;
                case DexpaContentType.Front:
                    name = "Вид спереди";
                    break;
                case DexpaContentType.Back:
                    name = "Вид сзади";
                    break;
                case DexpaContentType.Interior:
                    name = "Интерьер";
                    break;
                case DexpaContentType.RegNumber:
                    name = "Регистрационный номер";
                    break;
                case DexpaContentType.Side:
                    name = "Вид сбоку";
                    break;
                case DexpaContentType.CarDamages:
                    name = "Повреждения";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("contentType");
            }

            return new DexpaContentTypeDTO()
            {
                Name = name,
                Type = contentType
            };
        }
    }
}