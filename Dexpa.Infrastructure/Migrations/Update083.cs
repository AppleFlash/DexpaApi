namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update083 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.CustomerFeedbacks", "DriverId");
            AddForeignKey("dbo.CustomerFeedbacks", "DriverId", "dbo.Drivers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerFeedbacks", "DriverId", "dbo.Drivers");
            DropIndex("dbo.CustomerFeedbacks", new[] { "DriverId" });
        }
    }
}
