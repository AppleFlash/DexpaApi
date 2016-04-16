namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update064 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GlobalSettings", "QiwiCheckInterval", c => c.Int(nullable: false));
            AddColumn("dbo.GlobalSettings", "SmscLogin", c => c.String());
            AddColumn("dbo.GlobalSettings", "SmscPassword", c => c.String());
            AddColumn("dbo.GlobalSettings", "YandexTaxiHost", c => c.String());
            AddColumn("dbo.GlobalSettings", "TracksUrl", c => c.String());
            AddColumn("dbo.GlobalSettings", "RentTransactionTime", c => c.Int(nullable: false));
            AddColumn("dbo.GlobalSettings", "BalanceRecalculateTimeInterval", c => c.Int(nullable: false));
            AddColumn("dbo.GlobalSettings", "OrderRequestAdditionalTimeSec", c => c.Int(nullable: false));
            AddColumn("dbo.GlobalSettings", "ProcessName", c => c.String());
            AddColumn("dbo.GlobalSettings", "PauseBeforeNextOrderSec", c => c.Int(nullable: false));
            DropColumn("dbo.GlobalSettings", "QiwiCheckPeriod");
            DropColumn("dbo.GlobalSettings", "SMSLogin");
            DropColumn("dbo.GlobalSettings", "SMSPassword");
            DropColumn("dbo.GlobalSettings", "CarRentPeriod");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GlobalSettings", "CarRentPeriod", c => c.Int(nullable: false));
            AddColumn("dbo.GlobalSettings", "SMSPassword", c => c.String());
            AddColumn("dbo.GlobalSettings", "SMSLogin", c => c.String());
            AddColumn("dbo.GlobalSettings", "QiwiCheckPeriod", c => c.Int(nullable: false));
            DropColumn("dbo.GlobalSettings", "PauseBeforeNextOrderSec");
            DropColumn("dbo.GlobalSettings", "ProcessName");
            DropColumn("dbo.GlobalSettings", "OrderRequestAdditionalTimeSec");
            DropColumn("dbo.GlobalSettings", "BalanceRecalculateTimeInterval");
            DropColumn("dbo.GlobalSettings", "RentTransactionTime");
            DropColumn("dbo.GlobalSettings", "TracksUrl");
            DropColumn("dbo.GlobalSettings", "YandexTaxiHost");
            DropColumn("dbo.GlobalSettings", "SmscPassword");
            DropColumn("dbo.GlobalSettings", "SmscLogin");
            DropColumn("dbo.GlobalSettings", "QiwiCheckInterval");
        }
    }
}
