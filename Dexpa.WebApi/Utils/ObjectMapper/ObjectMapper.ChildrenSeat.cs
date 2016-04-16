using System;
using AutoMapper;
using Dexpa.Core.Model;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.WebApi.Utils
{
    public partial class ObjectMapper
    {
        private void CreatMapChildrenSeat()
        {
            Mapper.CreateMap<ChildrenSeat, ChildrenSeatDTO>()
                .ConvertUsing(ChildrenSeatDTO);

            Mapper.CreateMap<ChildrenSeatDTO, ChildrenSeat>()
                .ConvertUsing(cs => cs == null ? ChildrenSeat.None : cs.Type);
        }

        private ChildrenSeatDTO ChildrenSeatDTO(ChildrenSeat childrenSeat)
        {
            var name = string.Empty;
            switch (childrenSeat)
            {
                case ChildrenSeat.None:
                    name = "Нет";
                    break;
                case ChildrenSeat.Weight0_10:
                    name = "0 - 1 год, 0 - 10 кг";
                    break;
                case ChildrenSeat.Weight0_13:
                    name = "0 - 1.5 год, 0 - 13 кг";
                    break;
                case ChildrenSeat.Weight0_20:
                    name = "0 - 5 лет, 0 - 20 кг";
                    break;
                case ChildrenSeat.Weight0_25:
                    name = "0 - 7 лет, 0 - 25 кг";
                    break;
                case ChildrenSeat.Weight0_40:
                    name = "0 - 12 лет, 0 - 40 кг";
                    break;
                case ChildrenSeat.Weight9_18:
                    name = "1 - 4 года, 9 - 18 кг";
                    break;
                case ChildrenSeat.Weight9_36:
                    name = "1 - 10 лет, 9 - 36 кг";
                    break;
                case ChildrenSeat.Weight15_25:
                    name = "3 - 7 лет, 15 - 25 кг";
                    break;
                case ChildrenSeat.Weight22_36:
                    name = "6 - 10 лет, 22 - 36 кг";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("childrenSeat");
            }

            return new ChildrenSeatDTO
            {
                Name = name,
                Type = childrenSeat
            };
        }
    }
}