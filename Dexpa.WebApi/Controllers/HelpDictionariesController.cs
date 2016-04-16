using System;
using System.Collections.Generic;
using System.Web.Http;
using Dexpa.Core;
using Dexpa.Core.Model;
using Dexpa.DTO.HelpDictionaries;
using Dexpa.WebApi.Utils;

namespace Dexpa.WebApi.Controllers
{
    public class HelpDictionariesController : ApiControllerBase
    {
        [HttpGet]
        public List<ChildrenSeatDTO> ChildrenSeats()
        {
            return GetDictionary<ChildrenSeat, ChildrenSeatDTO>();
        }

        [HttpGet]
        public List<CarStatusDTO> CarStatuses()
        {
            return GetDictionary<CarStatus, CarStatusDTO>();
        }

        [HttpGet]
        public List<TransactionGroupDTO> TransactionGroups()
        {
            return GetDictionary<TransactionGroup, TransactionGroupDTO>();
        }

        [HttpGet]
        public List<PaymentMethodDTO> TransactionMethods()
        {
            return GetDictionary<PaymentMethod, PaymentMethodDTO>();
        }

        [HttpGet]
        public List<TransactionTypeDTO> TransactionTypes()
        {
            return GetDictionary<TransactionType, TransactionTypeDTO>();
        }

        [HttpGet]
        public List<OrderTypeDTO> OrderTypes()
        {
            return GetDictionary<OrderType, OrderTypeDTO>();
        }

        [HttpGet]
        public List<DriverStateDTO> DriverStates()
        {
            return GetDictionary<DriverState, DriverStateDTO>();
        }

        [HttpGet]
        public List<TariffZonesTypeDTO> GetTariffZones()
        {
            return GetDictionary<TariffZoneType, TariffZonesTypeDTO>();
        }

        [HttpGet]
        public List<OrderStateDTO> OrdersStates()
        {
            return GetDictionary<OrderStateType, OrderStateDTO>();
        }

        [HttpGet]
        public List<OrderSourceDTO> OrdersSources()
        {
            return GetDictionary<OrderSource, OrderSourceDTO>();
        }

        private List<T2> GetDictionary<T1, T2>() where T2 : new()
        {
            var dictionary = new List<T2>();
            foreach (T1 seatType in Enum.GetValues(typeof(T1)))
            {
                var item = ObjectMapper.Instance.Map<T1, T2>(seatType);
                dictionary.Add(item);
            }
            return dictionary;
        }
    }
}