namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update008 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Timestamp", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Timestamp");
        }
    }
}
