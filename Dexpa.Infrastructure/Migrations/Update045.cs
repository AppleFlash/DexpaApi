namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update045 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tariffs", "RegionIncludedFreeMinutes", c => c.Int(nullable: false));
            AddColumn("dbo.Tariffs", "AirportFreeWaiting", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tariffs", "AirportFreeWaiting");
            DropColumn("dbo.Tariffs", "RegionIncludedFreeMinutes");
        }
    }
}
