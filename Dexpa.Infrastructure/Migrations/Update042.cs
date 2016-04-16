namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update042 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderHistories", "Comment", c => c.String());
            AddColumn("dbo.OrderHistories", "ChangedProperty", c => c.Int(nullable: false));
            AddColumn("dbo.OrderHistories", "OldValues", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderHistories", "OldValues");
            DropColumn("dbo.OrderHistories", "ChangedProperty");
            DropColumn("dbo.OrderHistories", "Comment");
        }
    }
}
