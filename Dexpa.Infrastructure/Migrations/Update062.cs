namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Update062 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RobotLogs",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Timestamp = c.DateTime(nullable: false),
                    RobotEnabled = c.Boolean(nullable: false),
                    RobotDistance = c.Double(nullable: false),
                    RobotTime = c.Double(nullable: false),
                    OrderDistance = c.Double(),
                    OrderTime = c.Double(nullable: false),
                    OrderId = c.Long(nullable: false),
                    DriverId = c.Long(nullable: false),
                    IsDriverOptionsFit = c.Boolean(nullable: false),
                    IsDriverWorkAllowed = c.Boolean(nullable: false),
                    IsDriverSelected = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .Index(c => c.OrderId)
                .Index(c => c.DriverId);

            CreateIndex("dbo.TrackPoints", new[] { "Timestamp", "DriverId" });
        }

        public override void Down()
        {
            DropIndex("dbo.TrackPoints", new[] { "Timestamp", "DriverId" });
            DropIndex("dbo.RobotLogs", new[] { "DriverId" });
            DropIndex("dbo.RobotLogs", new[] { "OrderId" });
            DropTable("dbo.RobotLogs");
        }
    }
}
