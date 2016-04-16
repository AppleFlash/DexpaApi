using System.Data.Entity;
using Dexpa.Core;
using Dexpa.Core.Model;

namespace Dexpa.DataStorageSql
{
    public class ModelContext : DbContext
    {
        public DbSet<Customer> Customers { get; private set; }

        public DbSet<Order> Orders { get; private set; }

        public DbSet<Driver> Drivers { get; private set; } 

        //public DbSet<ICar> Cars { get; private set; }

        public ModelContext(string connectionString)
            :base(connectionString)
        {
        }
    }
}
