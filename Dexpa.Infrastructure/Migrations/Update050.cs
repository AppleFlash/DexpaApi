namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update050 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GlobalSettings", "LastQiwiCheckTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GlobalSettings", "LastQiwiCheckTime");
        }
    }
}
