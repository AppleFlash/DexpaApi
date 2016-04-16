using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Dexpa.Core.Model;
using Dexpa.Core.Model.Reports;
using Microsoft.AspNet.Identity.EntityFramework;
using NLog;

namespace Dexpa.Infrastructure
{
    public partial class ModelContext : IdentityDbContext<User>
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Driver> Drivers { get; set; }

        public DbSet<Car> Cars { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<SystemEvent> Events { get; set; }

        public DbSet<DriverWorkConditions> WorkConditionses { get; set; }

        public DbSet<Tariff> Tarifs { get; set; }

        public DbSet<OrderHistory> OrderHistories { get; set; }

        public DbSet<RegionPoint> RegionData { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<CustomerAddresses> CustomerAddresseses { get; set; }

        public DbSet<TrackPoint> TrackPoints { get; set; }

        public DbSet<Content> Contents { get; set; }

        public DbSet<OrderRequest> OrderRequests { get; set; }

        public DbSet<DriverOrderRequest> DriverOrderRequests { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        public DbSet<GlobalSettings> GlobalSettings { get; set; }

        public DbSet<IpPhoneUser> IpPhoneUsers { get; set; }

        public DbSet<RobotLog> RobotLogs { get; set; }

        public DbSet<WayBills> WayBillses { get; set; }

        public DbSet<OrderDriver> OrderDrivers { get; set; }

        public DbSet<CarEvent> CarEvents { get; set; }

        public DbSet<Repair> Repairs { get; set; }

        public DbSet<DriverScores> DriverScores { get; set; }

        public DbSet<CustomerFeedback> CustomerFeedbacks { get; set; }

        public DbSet<NewsMessage> NewsMessages { get; set; }

        public string Id { get; private set; }

        private static Logger mLogger = LogManager.GetCurrentClassLogger();

        public ModelContext()
            : base("DexpaDatabase")
        {
            Database.CommandTimeout = 600;
            Id = Guid.NewGuid().ToString();
        }

        public override int SaveChanges()
        {
            try
            {
                foreach (var driver in ChangeTracker.Entries<Driver>())
                {
                    if (driver.State == EntityState.Modified)
                    {
                        foreach (var propName in driver.CurrentValues.PropertyNames)
                        {
                            if (propName == "State")
                            {
                                var newValue = driver.CurrentValues.GetValue<DriverState>(propName);
                                var oldValue = driver.OriginalValues.GetValue<DriverState>(propName);
                                var driverId = driver.Entity.Id;

                                if (newValue != oldValue)
                                {
                                    var stackTrace = new StackTrace();
                                    mLogger.Debug("Someone changed driver {2} state from {0} to {1}. Stack trace: {3}", oldValue, newValue, driverId, stackTrace);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                mLogger.Error(exception);
            }

            return base.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }


    }
}
