namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update025 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Source", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "SourceOrderId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "SourceOrderId");
            DropColumn("dbo.Orders", "Source");
        }
    }
}
