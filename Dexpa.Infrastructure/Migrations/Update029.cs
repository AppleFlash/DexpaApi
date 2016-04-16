namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update029 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Transactions", "Driver_Id", "dbo.Drivers");
            DropIndex("dbo.Transactions", new[] { "Driver_Id" });
            RenameColumn(table: "dbo.Transactions", name: "Driver_Id", newName: "DriverId");
            AlterColumn("dbo.Transactions", "DriverId", c => c.Long(nullable: false));
            CreateIndex("dbo.Transactions", "DriverId");
            AddForeignKey("dbo.Transactions", "DriverId", "dbo.Drivers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "DriverId", "dbo.Drivers");
            DropIndex("dbo.Transactions", new[] { "DriverId" });
            AlterColumn("dbo.Transactions", "DriverId", c => c.Long());
            RenameColumn(table: "dbo.Transactions", name: "DriverId", newName: "Driver_Id");
            CreateIndex("dbo.Transactions", "Driver_Id");
            AddForeignKey("dbo.Transactions", "Driver_Id", "dbo.Drivers", "Id");
        }
    }
}
