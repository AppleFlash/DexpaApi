namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update021 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tariffs", "MinimumCost", c => c.Double(nullable: false));
            AddColumn("dbo.Tariffs", "FreeWaitingMinutes", c => c.Int(nullable: false));
            AddColumn("dbo.Tariffs", "PaidWaitingCost", c => c.Int(nullable: false));
            AddColumn("dbo.TariffZones", "MinuteCost", c => c.Double(nullable: false));
            AddColumn("dbo.TariffZones", "KilometerCost", c => c.Double(nullable: false));
            AddColumn("dbo.TariffZones", "MinVelocity", c => c.Int(nullable: false));
            DropColumn("dbo.Tariffs", "TimeFrom");
            DropColumn("dbo.Tariffs", "TimeTo");
            AddColumn("dbo.Tariffs", "TimeFrom", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Tariffs", "TimeTo", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.Tariffs", "IncludeMinutes", c => c.Int(nullable: false));
            AlterColumn("dbo.Tariffs", "IncludeKilometers", c => c.Int(nullable: false));
            DropColumn("dbo.Tariffs", "Minimum");
            DropColumn("dbo.Tariffs", "FreeWaiting");
            DropColumn("dbo.Tariffs", "PayWaiting");
            DropColumn("dbo.TariffZones", "Minutes");
            DropColumn("dbo.TariffZones", "Kilometers");
            DropColumn("dbo.TariffZones", "Velocity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TariffZones", "Velocity", c => c.Int(nullable: false));
            AddColumn("dbo.TariffZones", "Kilometers", c => c.Int(nullable: false));
            AddColumn("dbo.TariffZones", "Minutes", c => c.Int(nullable: false));
            AddColumn("dbo.Tariffs", "PayWaiting", c => c.Long(nullable: false));
            AddColumn("dbo.Tariffs", "FreeWaiting", c => c.Long(nullable: false));
            AddColumn("dbo.Tariffs", "Minimum", c => c.Long(nullable: false));
            AlterColumn("dbo.Tariffs", "IncludeKilometers", c => c.Long(nullable: false));
            AlterColumn("dbo.Tariffs", "IncludeMinutes", c => c.Long(nullable: false));
            DropColumn("dbo.Tariffs", "TimeFrom");
            DropColumn("dbo.Tariffs", "TimeTo");
            AddColumn("dbo.Tariffs", "TimeTo", c => c.Long(nullable: false));
            AddColumn("dbo.Tariffs", "TimeFrom", c => c.Long(nullable: false));
            DropColumn("dbo.TariffZones", "MinVelocity");
            DropColumn("dbo.TariffZones", "KilometerCost");
            DropColumn("dbo.TariffZones", "MinuteCost");
            DropColumn("dbo.Tariffs", "PaidWaitingCost");
            DropColumn("dbo.Tariffs", "FreeWaitingMinutes");
            DropColumn("dbo.Tariffs", "MinimumCost");
        }
    }
}
