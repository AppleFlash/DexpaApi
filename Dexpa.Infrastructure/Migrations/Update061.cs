namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update061 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TrackPoints", "DriverState", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TrackPoints", "DriverState");
        }
    }
}
