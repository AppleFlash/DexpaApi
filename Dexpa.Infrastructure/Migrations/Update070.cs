namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update070 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderDrivers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        OrderId = c.Long(nullable: false),
                        DriverId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drivers", t => t.DriverId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.DriverId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDrivers", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderDrivers", "DriverId", "dbo.Drivers");
            DropIndex("dbo.OrderDrivers", new[] { "DriverId" });
            DropIndex("dbo.OrderDrivers", new[] { "OrderId" });
            DropTable("dbo.OrderDrivers");
        }
    }
}
