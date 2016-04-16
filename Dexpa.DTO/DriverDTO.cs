using System;
using System.Collections.Generic;
using Dexpa.DTO.HelpDictionaries;

namespace Dexpa.DTO
{
    public class DriverDTO
    {
        public long Id { get; set; }

        //[RegularExpression(@"[@А-ЯЁа-яё]+", ErrorMessage = "Имя водителя может состоять только из букв русского алфавита")]
        public string FirstName { get; set; }

        //[RegularExpression(@"[@А-ЯЁа-яё\W]+", ErrorMessage = "Фамилия водителя может состоять только из букв русского алфавита")]
        public string LastName { get; set; }

        //[RegularExpression(@"[@А-ЯЁа-яё]+", ErrorMessage = "Отчество водителя может состоять только из букв русского алфавита")]
        public string MiddleName { get; set; }

        public IList<long> Phones { get; set; }

        public string Callsign { get; set; }

        public DriverStateDTO State { get; set; }

        public LocationDTO Location { get; set; }

        public CarDTO Car { get; set; }

        public DriverWorkConditionsDTO WorkConditions { get; set; }

        public double Balance { get; set; }

        public double BalanceLimit { get; set; }

        public double DayTimeFee { get; set; }

        public DaysDTO WorkSchedule { get; set; }

        public DateTime Timestamp { get; set; }

        public string UserName { get; set; }

        public string UserPassword { get; set; }

        public ContentsObjDTO Content { get; set; }

        public bool IsOnline { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public DriverLicenseDTO DriverLicense { get; set; }

        public RobotSettingsDTO RobotSettings { get; set; }

        public DateTime LastTrackUpdateTime { get; set; }

        public bool TechnicalSupport { get; set; }

        public bool OrderListsVisible { get; set; }

        public DriverDTO()
        {
        }
    }
}
