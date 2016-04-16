namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update058 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderRequests", "Type", c => c.Int(nullable: false));
            AlterColumn("dbo.Customers", "Phone", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "Phone", c => c.Long(nullable: false));
            DropColumn("dbo.OrderRequests", "Type");
        }
    }
}
