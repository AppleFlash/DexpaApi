namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update071 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RegionPoints", "Region_Id", "dbo.Regions");
            DropIndex("dbo.RegionPoints", new[] { "Region_Id" });
            RenameColumn(table: "dbo.RegionPoints", name: "Region_Id", newName: "RegionId");
            AlterColumn("dbo.RegionPoints", "RegionId", c => c.Long(nullable: false));
            CreateIndex("dbo.RegionPoints", "RegionId");
            AddForeignKey("dbo.RegionPoints", "RegionId", "dbo.Regions", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RegionPoints", "RegionId", "dbo.Regions");
            DropIndex("dbo.RegionPoints", new[] { "RegionId" });
            AlterColumn("dbo.RegionPoints", "RegionId", c => c.Long());
            RenameColumn(table: "dbo.RegionPoints", name: "RegionId", newName: "Region_Id");
            CreateIndex("dbo.RegionPoints", "Region_Id");
            AddForeignKey("dbo.RegionPoints", "Region_Id", "dbo.Regions", "Id");
        }
    }
}
