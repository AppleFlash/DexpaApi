namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update052 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OrderFees", "DriverWorkConditions_Id", c => c.Long(nullable: false));
            AddForeignKey("dbo.OrderFees", "DriverWorkConditions_Id", "dbo.DriverWorkConditions", "Id",
                cascadeDelete: true);
            CreateIndex("dbo.OrderFees", "DriverWorkConditions_Id");
        }

        public override void Down()
        {
            DropIndex("dbo.OrderFees", new[] { "DriverWorkConditions_Id" });
            DropForeignKey("dbo.OrderFees", "DriverWorkConditions_Id", "dbo.DriverWorkConditions");
            AlterColumn("dbo.OrderFees", "DriverWorkConditions_Id", c => c.Long());
        }
    }
}
