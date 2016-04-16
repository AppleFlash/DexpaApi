namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update034 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TariffRegionCosts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TariffId = c.Long(nullable: false),
                        RegionFromId = c.Long(nullable: false),
                        RegionToId = c.Long(nullable: false),
                        Cost = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Regions", t => t.RegionFromId, cascadeDelete: false)
                .ForeignKey("dbo.Regions", t => t.RegionToId, cascadeDelete: false)
                .ForeignKey("dbo.Tariffs", t => t.TariffId, cascadeDelete: false)
                .Index(t => t.TariffId)
                .Index(t => t.RegionFromId)
                .Index(t => t.RegionToId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TariffRegionCosts", "TariffId", "dbo.Tariffs");
            DropForeignKey("dbo.TariffRegionCosts", "RegionToId", "dbo.Regions");
            DropForeignKey("dbo.TariffRegionCosts", "RegionFromId", "dbo.Regions");
            DropIndex("dbo.TariffRegionCosts", new[] { "RegionToId" });
            DropIndex("dbo.TariffRegionCosts", new[] { "RegionFromId" });
            DropIndex("dbo.TariffRegionCosts", new[] { "TariffId" });
            DropTable("dbo.TariffRegionCosts");
        }
    }
}
