namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update017 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tariffs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Abbreviation = c.String(),
                        TimeFrom = c.Long(nullable: false),
                        TimeTo = c.Long(nullable: false),
                        Days = c.Int(nullable: false),
                        Minimum = c.Long(nullable: false),
                        IncludeMinutes = c.Long(nullable: false),
                        IncludeKilometers = c.Long(nullable: false),
                        FreeWaiting = c.Long(nullable: false),
                        PayWaiting = c.Long(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TariffZones",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TariffZoneType = c.Int(nullable: false),
                        Minutes = c.Int(nullable: false),
                        Kilometers = c.Int(nullable: false),
                        Velocity = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Tariff_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tariffs", t => t.Tariff_Id)
                .Index(t => t.Tariff_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TariffZones", "Tariff_Id", "dbo.Tariffs");
            DropIndex("dbo.TariffZones", new[] { "Tariff_Id" });
            DropTable("dbo.TariffZones");
            DropTable("dbo.Tariffs");
        }
    }
}
