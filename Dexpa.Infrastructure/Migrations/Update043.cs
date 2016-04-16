namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update043 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderHistories", "MessageType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderHistories", "MessageType");
        }
    }
}
