namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update080 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerFeedbacks",
                c => new
                    {
                        OrderId = c.Long(nullable: false, identity: true),
                        DriverId = c.Long(nullable: false),
                        Score = c.Short(nullable: false),
                        Comment = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.OrderId);
            
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DriverScores");
            DropTable("dbo.CustomerFeedbacks");
        }
    }
}
