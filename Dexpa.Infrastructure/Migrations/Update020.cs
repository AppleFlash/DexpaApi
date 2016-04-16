namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update020 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Drivers", name: "Car_Id", newName: "CarId");
            RenameColumn(table: "dbo.Drivers", name: "WorkConditions_Id", newName: "WorkConditionsId");
            RenameColumn(table: "dbo.Orders", name: "Customer_Id", newName: "CustomerId");
            RenameColumn(table: "dbo.Orders", name: "Driver_Id", newName: "DriverId");
            RenameIndex(table: "dbo.Drivers", name: "IX_Car_Id", newName: "IX_CarId");
            RenameIndex(table: "dbo.Drivers", name: "IX_WorkConditions_Id", newName: "IX_WorkConditionsId");
            RenameIndex(table: "dbo.Orders", name: "IX_Driver_Id", newName: "IX_DriverId");
            RenameIndex(table: "dbo.Orders", name: "IX_Customer_Id", newName: "IX_CustomerId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Orders", name: "IX_CustomerId", newName: "IX_Customer_Id");
            RenameIndex(table: "dbo.Orders", name: "IX_DriverId", newName: "IX_Driver_Id");
            RenameIndex(table: "dbo.Drivers", name: "IX_WorkConditionsId", newName: "IX_WorkConditions_Id");
            RenameIndex(table: "dbo.Drivers", name: "IX_CarId", newName: "IX_Car_Id");
            RenameColumn(table: "dbo.Orders", name: "DriverId", newName: "Driver_Id");
            RenameColumn(table: "dbo.Orders", name: "CustomerId", newName: "Customer_Id");
            RenameColumn(table: "dbo.Drivers", name: "WorkConditionsId", newName: "WorkConditions_Id");
            RenameColumn(table: "dbo.Drivers", name: "CarId", newName: "Car_Id");
        }
    }
}
