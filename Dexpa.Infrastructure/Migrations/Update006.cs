namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update006 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Brand = c.String(),
                        Model = c.String(),
                        Callsign = c.String(),
                        Status = c.Int(nullable: false),
                        RegNumber = c.String(),
                        ProductionYear = c.Int(nullable: false),
                        CarClass = c.Int(nullable: false),
                        Color = c.String(),
                        ChildrenSeat = c.Int(nullable: false),
                        Features = c.Int(nullable: false),
                        Description = c.String(),
                        Permission_Number = c.Int(nullable: false),
                        Permission_Series = c.String(),
                        Permission_Number2 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Drivers", "Car_Id", c => c.Long());
            CreateIndex("dbo.Drivers", "Car_Id");
            AddForeignKey("dbo.Drivers", "Car_Id", "dbo.Cars", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Drivers", "Car_Id", "dbo.Cars");
            DropIndex("dbo.Drivers", new[] { "Car_Id" });
            DropColumn("dbo.Drivers", "Car_Id");
            DropTable("dbo.Cars");
        }
    }
}
