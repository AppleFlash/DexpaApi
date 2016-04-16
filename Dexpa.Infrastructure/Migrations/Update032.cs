namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update032 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "TariffId", c => c.Long());
            CreateIndex("dbo.Orders", "TariffId");
            AddForeignKey("dbo.Orders", "TariffId", "dbo.Tariffs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "TariffId", "dbo.Tariffs");
            DropIndex("dbo.Orders", new[] { "TariffId" });
            DropColumn("dbo.Orders", "TariffId");
        }
    }
}
