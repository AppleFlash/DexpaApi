namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update077 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerAddresses", "Address_IsAirport", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "FromAddress_IsAirport", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "ToAddress_IsAirport", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "ToAddress_IsAirport");
            DropColumn("dbo.Orders", "FromAddress_IsAirport");
            DropColumn("dbo.CustomerAddresses", "Address_IsAirport");
        }
    }
}
