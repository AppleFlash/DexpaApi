namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update036 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderRequests",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        OrderUid = c.String(),
                        DataJson = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Orders", "Comments", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Comments");
            DropTable("dbo.OrderRequests");
        }
    }
}
