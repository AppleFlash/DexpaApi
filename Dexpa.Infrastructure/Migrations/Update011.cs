namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update011 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DriverWorkConditions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderFees",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OrderType = c.Int(nullable: false),
                        Value = c.Double(nullable: false),
                        FeeType = c.Int(nullable: false),
                        DriverWorkConditions_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DriverWorkConditions", t => t.DriverWorkConditions_Id)
                .Index(t => t.DriverWorkConditions_Id);
            
            AddColumn("dbo.Drivers", "WorkConditions_Id", c => c.Long());
            CreateIndex("dbo.Drivers", "WorkConditions_Id");
            AddForeignKey("dbo.Drivers", "WorkConditions_Id", "dbo.DriverWorkConditions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Drivers", "WorkConditions_Id", "dbo.DriverWorkConditions");
            DropForeignKey("dbo.OrderFees", "DriverWorkConditions_Id", "dbo.DriverWorkConditions");
            DropIndex("dbo.OrderFees", new[] { "DriverWorkConditions_Id" });
            DropIndex("dbo.Drivers", new[] { "WorkConditions_Id" });
            DropColumn("dbo.Drivers", "WorkConditions_Id");
            DropTable("dbo.OrderFees");
            DropTable("dbo.DriverWorkConditions");
        }
    }
}
