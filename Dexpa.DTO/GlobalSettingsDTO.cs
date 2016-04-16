using System;
using Dexpa.Core.Model;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.DTO
{
    public class GlobalSettingsDTO
    {
        public double TechnicalSupportFeeSize { get; set; }

        public string QiwiLogin { get; set; }
        public string QiwiPassword { get; set; }
        public int QiwiCheckInterval { get; set; } // минуты

        public string SmscLogin { get; set; }
        public string SmscPassword { get; set; }

        public string YandexTaxiHost { get; set; }
        public string YandexClid { get; set; }
        public string YandexApiKey { get; set; }

        public TimeSpan RentTransactionTime { get; set; }

        public int HighPriorityOrderTime { get; set; } // минуты

        public string YandexCabLogin { get; set; }

        public string YandexCabPassword { get; set; }

        public string YandexCabId { get; set; }
    }
}
