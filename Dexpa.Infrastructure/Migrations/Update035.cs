namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update035 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AspNetUsers", name: "Driver_Id", newName: "DriverId");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_Driver_Id", newName: "IX_DriverId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_DriverId", newName: "IX_Driver_Id");
            RenameColumn(table: "dbo.AspNetUsers", name: "DriverId", newName: "Driver_Id");
        }
    }
}
