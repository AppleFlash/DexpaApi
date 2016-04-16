namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update072 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Contents", "DriverId", "dbo.Drivers");
            DropIndex("dbo.Contents", new[] { "DriverId" });
            CreateTable(
                "dbo.CarEvents",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        Name = c.String(maxLength: 25),
                        Comment = c.String(maxLength: 256),
                        CarId = c.Long(nullable: false),
                        ImplementedById = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ImplementedById)
                .Index(t => t.ImplementedById);
            
            CreateTable(
                "dbo.Repairs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        Cost = c.Single(nullable: false),
                        Comment = c.String(maxLength: 256),
                        ImplementedById = c.String(maxLength: 128),
                        CarId = c.Long(nullable: false),
                        GuiltyDriverId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drivers", t => t.GuiltyDriverId)
                .ForeignKey("dbo.AspNetUsers", t => t.ImplementedById)
                .Index(t => t.ImplementedById)
                .Index(t => t.GuiltyDriverId);
            
            CreateTable(
                "dbo.WayBills",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        DriverId = c.Long(nullable: false),
                        CarId = c.Long(nullable: false),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(nullable: false),
                        StartMileage = c.Int(nullable: false),
                        EndMileage = c.Int(nullable: false),
                        Responsible = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cars", t => t.CarId, cascadeDelete: true)
                .ForeignKey("dbo.Drivers", t => t.DriverId, cascadeDelete: true)
                .Index(t => t.DriverId)
                .Index(t => t.CarId);
            
            AddColumn("dbo.Contents", "RepairId", c => c.Long());
            AlterColumn("dbo.Contents", "DriverId", c => c.Long());
            CreateIndex("dbo.Contents", "DriverId");
            CreateIndex("dbo.Contents", "RepairId");
            AddForeignKey("dbo.Contents", "RepairId", "dbo.Repairs", "Id");
            AddForeignKey("dbo.Contents", "DriverId", "dbo.Drivers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contents", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.WayBills", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.WayBills", "CarId", "dbo.Cars");
            DropForeignKey("dbo.Repairs", "ImplementedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Repairs", "GuiltyDriverId", "dbo.Drivers");
            DropForeignKey("dbo.Contents", "RepairId", "dbo.Repairs");
            DropForeignKey("dbo.CarEvents", "ImplementedById", "dbo.AspNetUsers");
            DropIndex("dbo.WayBills", new[] { "CarId" });
            DropIndex("dbo.WayBills", new[] { "DriverId" });
            DropIndex("dbo.Repairs", new[] { "GuiltyDriverId" });
            DropIndex("dbo.Repairs", new[] { "ImplementedById" });
            DropIndex("dbo.Contents", new[] { "RepairId" });
            DropIndex("dbo.Contents", new[] { "DriverId" });
            DropIndex("dbo.CarEvents", new[] { "ImplementedById" });
            AlterColumn("dbo.Contents", "DriverId", c => c.Long(nullable: false));
            DropColumn("dbo.Contents", "RepairId");
            DropTable("dbo.WayBills");
            DropTable("dbo.Repairs");
            DropTable("dbo.CarEvents");
            CreateIndex("dbo.Contents", "DriverId");
            AddForeignKey("dbo.Contents", "DriverId", "dbo.Drivers", "Id", cascadeDelete: true);
        }
    }
}
