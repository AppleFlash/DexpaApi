namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update073 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.GlobalSettings", "YandexTaxiHost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GlobalSettings", "YandexTaxiHost", c => c.String());
        }
    }
}
