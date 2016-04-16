namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update085 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerFeedbacks",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OrderId = c.Long(nullable: false),
                        DriverId = c.Long(nullable: false),
                        CustomerId = c.Long(),
                        Date = c.DateTime(nullable: false),
                        Score = c.Short(nullable: false),
                        Comment = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Drivers", t => t.DriverId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.DriverId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.DriverScores",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DriverId = c.Long(nullable: false),
                        Total = c.Double(nullable: false),
                        AverageClientScore = c.Double(),
                        ClientFeedbacksCount = c.Double(),
                        DriverLateScore = c.Int(),
                        CancelledOrders = c.Int(),
                        FakeWaitings = c.Int(),
                        TrackQuality = c.Int(),
                        ExamResult = c.Int(),
                        ExamDate = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drivers", t => t.DriverId, cascadeDelete: true)
                .Index(t => t.DriverId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DriverScores", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.CustomerFeedbacks", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.CustomerFeedbacks", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.CustomerFeedbacks", "CustomerId", "dbo.Customers");
            DropIndex("dbo.DriverScores", new[] { "DriverId" });
            DropIndex("dbo.CustomerFeedbacks", new[] { "CustomerId" });
            DropIndex("dbo.CustomerFeedbacks", new[] { "DriverId" });
            DropIndex("dbo.CustomerFeedbacks", new[] { "OrderId" });
            DropTable("dbo.DriverScores");
            DropTable("dbo.CustomerFeedbacks");
        }
    }
}
