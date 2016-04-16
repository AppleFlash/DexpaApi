namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update060 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerAddresses", "Address_Longitude", c => c.Double());
            AddColumn("dbo.CustomerAddresses", "Address_Latitude", c => c.Double());
            AddColumn("dbo.Orders", "FromAddress_Longitude", c => c.Double());
            AddColumn("dbo.Orders", "FromAddress_Latitude", c => c.Double());
            AddColumn("dbo.Orders", "ToAddress_Longitude", c => c.Double());
            AddColumn("dbo.Orders", "ToAddress_Latitude", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "ToAddress_Latitude");
            DropColumn("dbo.Orders", "ToAddress_Longitude");
            DropColumn("dbo.Orders", "FromAddress_Latitude");
            DropColumn("dbo.Orders", "FromAddress_Longitude");
            DropColumn("dbo.CustomerAddresses", "Address_Latitude");
            DropColumn("dbo.CustomerAddresses", "Address_Longitude");
        }
    }
}
