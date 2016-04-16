namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update084 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CustomerFeedbacks", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.CustomerFeedbacks", "DriverId", "dbo.Drivers");
            DropIndex("dbo.CustomerFeedbacks", new[] { "DriverId" });
            DropIndex("dbo.CustomerFeedbacks", new[] { "CustomerId" });
            DropTable("dbo.CustomerFeedbacks");
            DropTable("dbo.DriverScores");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DriverScores",
                c => new
                    {
                        DriverId = c.Long(nullable: false, identity: true),
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
                .PrimaryKey(t => t.DriverId);
            
            CreateTable(
                "dbo.CustomerFeedbacks",
                c => new
                    {
                        OrderId = c.Long(nullable: false, identity: true),
                        DriverId = c.Long(nullable: false),
                        CustomerId = c.Long(),
                        Date = c.DateTime(nullable: false),
                        Score = c.Short(nullable: false),
                        Comment = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.OrderId);
            
            CreateIndex("dbo.CustomerFeedbacks", "CustomerId");
            CreateIndex("dbo.CustomerFeedbacks", "DriverId");
            AddForeignKey("dbo.CustomerFeedbacks", "DriverId", "dbo.Drivers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CustomerFeedbacks", "CustomerId", "dbo.Customers", "Id");
        }
    }
}
