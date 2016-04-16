namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update026 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RegionPoints",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Lat = c.Double(nullable: false),
                        Lng = c.Double(nullable: false),
                        Region_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Regions", t => t.Region_Id)
                .Index(t => t.Region_Id);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RegionPoints", "Region_Id", "dbo.Regions");
            DropIndex("dbo.RegionPoints", new[] { "Region_Id" });
            DropTable("dbo.Regions");
            DropTable("dbo.RegionPoints");
        }
    }
}
