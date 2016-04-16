using System.Data.Entity.Migrations;

namespace Dexpa.Infrastructure.Migrations
{
    public partial class Update001 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "Phones", c => c.String());
            AddColumn("dbo.Orders", "Customer_Id", c => c.Long());
            AddColumn("dbo.Orders", "Driver_Id", c => c.Long());
            CreateIndex("dbo.Orders", "Customer_Id");
            CreateIndex("dbo.Orders", "Driver_Id");
            AddForeignKey("dbo.Orders", "Customer_Id", "dbo.Customers", "Id");
            AddForeignKey("dbo.Orders", "Driver_Id", "dbo.Drivers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "Driver_Id", "dbo.Drivers");
            DropForeignKey("dbo.Orders", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.Orders", new[] { "Driver_Id" });
            DropIndex("dbo.Orders", new[] { "Customer_Id" });
            DropColumn("dbo.Orders", "Driver_Id");
            DropColumn("dbo.Orders", "Customer_Id");
            DropColumn("dbo.Drivers", "Phones");
        }
    }
}
