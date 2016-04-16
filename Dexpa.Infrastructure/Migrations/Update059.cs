namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update059 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GlobalSettings", "QiwiLogin", c => c.String());
            AddColumn("dbo.GlobalSettings", "QiwiPassword", c => c.String());
            AddColumn("dbo.GlobalSettings", "QiwiCheckPeriod", c => c.Int(nullable: false));
            AddColumn("dbo.GlobalSettings", "SMSLogin", c => c.String());
            AddColumn("dbo.GlobalSettings", "SMSPassword", c => c.String());
            AddColumn("dbo.GlobalSettings", "YandexClid", c => c.String());
            AddColumn("dbo.GlobalSettings", "YandexApiKey", c => c.String());
            AddColumn("dbo.GlobalSettings", "CarRentPeriod", c => c.Int(nullable: false));
            AddColumn("dbo.GlobalSettings", "HighPriorityOrderTime", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GlobalSettings", "HighPriorityOrderTime");
            DropColumn("dbo.GlobalSettings", "CarRentPeriod");
            DropColumn("dbo.GlobalSettings", "YandexApiKey");
            DropColumn("dbo.GlobalSettings", "YandexClid");
            DropColumn("dbo.GlobalSettings", "SMSPassword");
            DropColumn("dbo.GlobalSettings", "SMSLogin");
            DropColumn("dbo.GlobalSettings", "QiwiCheckPeriod");
            DropColumn("dbo.GlobalSettings", "QiwiPassword");
            DropColumn("dbo.GlobalSettings", "QiwiLogin");
        }
    }
}
