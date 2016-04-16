namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Update075 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Orders", new[] { "Timestamp", "DriverId", "Source" });
            CreateIndex("dbo.RobotLogs", new[] { "Timestamp", "DriverId", "OrderId" });
        }

        public override void Down()
        {
            DropIndex("dbo.RobotLogs", new[] { "Timestamp", "DriverId", "OrderId" });
            DropIndex("dbo.Orders", new[] { "Timestamp", "DriverId", "Source" });
        }
    }
}
