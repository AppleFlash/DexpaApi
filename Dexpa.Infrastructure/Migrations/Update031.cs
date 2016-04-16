namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update031 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SystemEvents", "OrderState", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SystemEvents", "OrderState");
        }
    }
}
