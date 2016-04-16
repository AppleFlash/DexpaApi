namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update063 : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.RobotLogs", "DriverId", "dbo.Drivers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RobotLogs", "OrderId", "dbo.Orders", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RobotLogs", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.RobotLogs", "DriverId", "dbo.Drivers");
        }
    }
}
