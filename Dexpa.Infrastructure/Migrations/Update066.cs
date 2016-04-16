namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update066 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RobotLogs", "DriverState", c => c.Int(nullable: false));
            AddColumn("dbo.RobotLogs", "IsDriverOnline", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RobotLogs", "IsDriverOnline");
            DropColumn("dbo.RobotLogs", "DriverState");
        }
    }
}
