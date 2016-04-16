namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update028 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TrackPoints",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Speed = c.Double(nullable: false),
                        Direction = c.Int(nullable: false),
                        DriverId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drivers", t => t.DriverId, cascadeDelete: true)
                .Index(t => t.DriverId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrackPoints", "DriverId", "dbo.Drivers");
            DropIndex("dbo.TrackPoints", new[] { "DriverId" });
            DropTable("dbo.TrackPoints");
        }
    }
}
