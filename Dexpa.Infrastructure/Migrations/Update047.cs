namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Update047 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        TariffId = c.Long(),
                        DateFrom = c.DateTime(nullable: false),
                        DateTo = c.DateTime(nullable: false),
                        Codeword = c.String(),
                        SlipNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tariffs", t => t.TariffId)
                .Index(t => t.TariffId);

            CreateTable(
                "dbo.DriverOrderRequests",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        DriverId = c.Long(nullable: false),
                        OrderId = c.Long(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drivers", t => t.DriverId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.DriverId)
                .Index(t => t.OrderId);

            CreateTable(
                "dbo.GlobalSettings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LastRentCheckTime = c.DateTime(nullable: false),
                        Version = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.Customers", "OrganizationId", c => c.Long());
            AddColumn("dbo.Drivers", "RobotSettings_Enabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.Drivers", "RobotSettings_OrderRadius", c => c.Int(nullable: false));
            AddColumn("dbo.Drivers", "RobotSettings_Airports", c => c.Boolean(nullable: false));
            AddColumn("dbo.Drivers", "RobotSettings_OrdersSequence", c => c.Boolean(nullable: false));
            AddColumn("dbo.Drivers", "RobotSettings_WantToHome", c => c.Boolean(nullable: false));
            AddColumn("dbo.Drivers", "RobotSettings_AddressSearch", c => c.String());
            AddColumn("dbo.Drivers", "RobotSettings_MinutesDepartureTime", c => c.Int(nullable: false));
            AddColumn("dbo.Transactions", "OrderId", c => c.Long());
            AlterColumn("dbo.Drivers", "WorkSchedule", c => c.Int(nullable: true));
            CreateIndex("dbo.Customers", "OrganizationId");
            CreateIndex("dbo.Transactions", "OrderId");
            AddForeignKey("dbo.Customers", "OrganizationId", "dbo.Organizations", "Id");
            AddForeignKey("dbo.Transactions", "OrderId", "dbo.Orders", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.DriverOrderRequests", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.DriverOrderRequests", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.Customers", "OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.Organizations", "TariffId", "dbo.Tariffs");
            DropIndex("dbo.Transactions", new[] { "OrderId" });
            DropIndex("dbo.DriverOrderRequests", new[] { "OrderId" });
            DropIndex("dbo.DriverOrderRequests", new[] { "DriverId" });
            DropIndex("dbo.Organizations", new[] { "TariffId" });
            DropIndex("dbo.Customers", new[] { "OrganizationId" });
            AlterColumn("dbo.Drivers", "WorkSchedule", c => c.String());
            DropColumn("dbo.Transactions", "OrderId");
            DropColumn("dbo.Drivers", "RobotSettings_MinutesDepartureTime");
            DropColumn("dbo.Drivers", "RobotSettings_AddressSearch");
            DropColumn("dbo.Drivers", "RobotSettings_WantToHome");
            DropColumn("dbo.Drivers", "RobotSettings_OrdersSequence");
            DropColumn("dbo.Drivers", "RobotSettings_Airports");
            DropColumn("dbo.Drivers", "RobotSettings_OrderRadius");
            DropColumn("dbo.Drivers", "RobotSettings_Enabled");
            DropColumn("dbo.Customers", "OrganizationId");
            DropTable("dbo.GlobalSettings");
            DropTable("dbo.DriverOrderRequests");
            DropTable("dbo.Organizations");
        }
    }
}
