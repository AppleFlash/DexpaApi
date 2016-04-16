namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update018 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "WorkSchedule", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Drivers", "WorkSchedule");
        }
    }
}
