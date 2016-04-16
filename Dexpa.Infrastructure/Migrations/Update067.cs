namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update067 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GlobalSettings", "RentTransactionTime", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.GlobalSettings", "Version");
            DropColumn("dbo.GlobalSettings", "TracksUrl");
            DropColumn("dbo.GlobalSettings", "BalanceRecalculateTimeInterval");
            DropColumn("dbo.GlobalSettings", "OrderRequestAdditionalTimeSec");
            DropColumn("dbo.GlobalSettings", "ProcessName");
            DropColumn("dbo.GlobalSettings", "PauseBeforeNextOrderSec");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GlobalSettings", "PauseBeforeNextOrderSec", c => c.Int(nullable: false));
            AddColumn("dbo.GlobalSettings", "ProcessName", c => c.String());
            AddColumn("dbo.GlobalSettings", "OrderRequestAdditionalTimeSec", c => c.Int(nullable: false));
            AddColumn("dbo.GlobalSettings", "BalanceRecalculateTimeInterval", c => c.String());
            AddColumn("dbo.GlobalSettings", "TracksUrl", c => c.String());
            AddColumn("dbo.GlobalSettings", "Version", c => c.String());
            AlterColumn("dbo.GlobalSettings", "RentTransactionTime", c => c.String());
        }
    }
}
