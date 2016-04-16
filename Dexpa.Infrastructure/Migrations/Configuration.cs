using System;
using System.Data.Entity.Migrations;
using System.Linq;
using Dexpa.Core.Model;
using Dexpa.Infrastructure.Utils;

namespace Dexpa.Infrastructure.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ModelContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ModelContext context)
        {
            context.Regions.AddOrUpdate(r => r.Name, MapRegionsSeed.SeedRegions());

            if (!context.GlobalSettings.Any())
            {
                var settings = new GlobalSettings();

                settings.RentTransactionTime = new TimeSpan(20, 0, 0);
                settings.QiwiCheckInterval = 30; // 30 минут
                settings.HighPriorityOrderTime = 60; //минут
                
                context.GlobalSettings.AddOrUpdate(settings);
            }
        }
    }
}
