namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update005 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "Location_Longitude", c => c.Double(nullable: false));
            AddColumn("dbo.Drivers", "Location_Latitude", c => c.Double(nullable: false));
            AddColumn("dbo.Drivers", "Location_Speed", c => c.Double(nullable: false));
            AddColumn("dbo.Drivers", "Location_Direction", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Drivers", "Location_Direction");
            DropColumn("dbo.Drivers", "Location_Speed");
            DropColumn("dbo.Drivers", "Location_Latitude");
            DropColumn("dbo.Drivers", "Location_Longitude");
        }
    }
}
