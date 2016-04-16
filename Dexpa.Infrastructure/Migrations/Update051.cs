namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update051 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderFees", "DriverWorkConditions_Id", "dbo.DriverWorkConditions");
            DropIndex("dbo.OrderFees", new[] { "DriverWorkConditions_Id" });
        }
        
        public override void Down()
        {
            AddForeignKey("dbo.OrderFees", "DriverWorkConditions_Id", "dbo.DriverWorkConditions", "Id");
            CreateIndex("dbo.OrderFees", "DriverWorkConditions_Id");
        }
    }
}
