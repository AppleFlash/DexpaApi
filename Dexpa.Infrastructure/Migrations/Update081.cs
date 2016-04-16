namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update081 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerFeedbacks", "CustomerId", c => c.Long());
            AddColumn("dbo.CustomerFeedbacks", "Date", c => c.DateTime(nullable: false));
            CreateIndex("dbo.CustomerFeedbacks", "CustomerId");
            AddForeignKey("dbo.CustomerFeedbacks", "CustomerId", "dbo.Customers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerFeedbacks", "CustomerId", "dbo.Customers");
            DropIndex("dbo.CustomerFeedbacks", new[] { "CustomerId" });
            DropColumn("dbo.CustomerFeedbacks", "Date");
            DropColumn("dbo.CustomerFeedbacks", "CustomerId");
        }
    }
}
