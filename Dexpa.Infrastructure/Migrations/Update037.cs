namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update037 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contents", "TimeStamp", c => c.DateTime(nullable: false));
            AddColumn("dbo.Drivers", "LastTrackUpdateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Drivers", "LastTrackUpdateTime");
            DropColumn("dbo.Contents", "TimeStamp");
        }
    }
}
