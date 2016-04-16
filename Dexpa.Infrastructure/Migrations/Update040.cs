namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update040 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "MinCost", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "MinCost");
        }
    }
}
