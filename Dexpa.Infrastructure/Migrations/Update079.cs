namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update079 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GlobalSettings", "YandexCabLogin", c => c.String());
            AddColumn("dbo.GlobalSettings", "YandexCabPassword", c => c.String());
            AddColumn("dbo.GlobalSettings", "YandexCabId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GlobalSettings", "YandexCabId");
            DropColumn("dbo.GlobalSettings", "YandexCabPassword");
            DropColumn("dbo.GlobalSettings", "YandexCabLogin");
        }
    }
}
