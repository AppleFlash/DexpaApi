using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace Dexpa.Core.Model
{
    public class Driver
    {
        private const int ONLINE_TIME_MINUTES = 3;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string Phones { get; set; }

        public DriverState State { get; set; }

        public Location Location { get; set; }

        public virtual Car Car { get; internal set; }

        public long? CarId { get; set; }

        public virtual DriverWorkConditions WorkConditions { get; internal set; }

        public long? WorkConditionsId { get; set; }

        public double Balance { get; set; }

        public double BalanceLimit { get; set; }

        public double DayTimeFee { get; set; }

        public DaysEnum WorkSchedule { get; set; }

        public DateTime Timestamp { get; set; }

        public DateTime LastTrackUpdateTime { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public DriverLicense DriverLicense { get; set; }

        public RobotSettings RobotSettings { get; set; }

        public bool TechnicalSupport { get; set; }

        public virtual ICollection<Repair> Repairs { get; set; }

        public virtual ICollection<Content> Content { get; set; }

        public virtual ICollection<User> Logins { get; set; }

        public bool OrderListsVisible
        {
            get { return false; }
        }

        [NotMapped]
        public bool IsOnline
        {
            get
            {
                return (DateTime.UtcNow - LastTrackUpdateTime).TotalMinutes < ONLINE_TIME_MINUTES;
            }
        }

        [NotMapped]
        public string Uuid
        {
            get { return Id.ToString(); }
        }

        public Driver()
        {
            Location = new Location();
            Timestamp = DateTime.UtcNow;
            LastTrackUpdateTime = SqlDateTime.MinValue.Value;
            RobotSettings = new RobotSettings();
        }

        public bool IsWorkAllowed()
        {
            return Balance > BalanceLimit
                && State != DriverState.Fired
                && State != DriverState.Blocked;
        }

        public bool IsFitOrder(OrderOptions orderOptions)
        {
            return Utils.Utils.IsDriverFitOrder(this, orderOptions);
        }
    }
}
