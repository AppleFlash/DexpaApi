namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update019 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "Timestamp", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Drivers", "Timestamp");
        }
    }
}
