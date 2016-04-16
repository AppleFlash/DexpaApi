using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dexpa.Core.Model
{
    public class GlobalSettings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime LastRentCheckTime { get; set; }

        public DateTime LastQiwiCheckTime { get; set; }

        public double TechnicalSupportFeeSize { get; set; }

        public string QiwiLogin { get; set; }

        public string QiwiPassword { get; set; }

        public int QiwiCheckInterval { get; set; } // минуты

        public string SmscLogin { get; set; }

        public string SmscPassword { get; set; }

        public string YandexClid { get; set; }

        public string YandexApiKey { get; set; }

        public TimeSpan RentTransactionTime { get; set; }

        public int HighPriorityOrderTime { get; set; } // минуты

        [NotMapped]
        public double QiwiFee { get; set; }

        public string YandexCabLogin { get; set; }

        public string YandexCabPassword { get; set; }

        public string YandexCabId { get; set; }

        public GlobalSettings()
        {
            LastRentCheckTime = new DateTime(2000, 1, 1);
            LastQiwiCheckTime = new DateTime(2000, 1, 1);
            RentTransactionTime = new TimeSpan(20, 0, 0);
            QiwiFee = 0.03;
        }
    }
}
