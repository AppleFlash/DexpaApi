namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update038 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TariffRegionCosts", "TariffId", "dbo.Tariffs");
            DropIndex("dbo.TariffRegionCosts", new[] { "TariffId" });
            RenameColumn(table: "dbo.TariffRegionCosts", name: "TariffId", newName: "Tariff_Id");
            AlterColumn("dbo.TariffRegionCosts", "Tariff_Id", c => c.Long());
            CreateIndex("dbo.TariffRegionCosts", "Tariff_Id");
            AddForeignKey("dbo.TariffRegionCosts", "Tariff_Id", "dbo.Tariffs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TariffRegionCosts", "Tariff_Id", "dbo.Tariffs");
            DropIndex("dbo.TariffRegionCosts", new[] { "Tariff_Id" });
            AlterColumn("dbo.TariffRegionCosts", "Tariff_Id", c => c.Long(nullable: false));
            RenameColumn(table: "dbo.TariffRegionCosts", name: "Tariff_Id", newName: "TariffId");
            CreateIndex("dbo.TariffRegionCosts", "TariffId");
            AddForeignKey("dbo.TariffRegionCosts", "TariffId", "dbo.Tariffs", "Id", cascadeDelete: true);
        }
    }
}
