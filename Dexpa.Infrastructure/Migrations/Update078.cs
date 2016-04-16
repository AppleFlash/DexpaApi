namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update078 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "PreliminaryCost", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "PreliminaryCost");
        }
    }
}
