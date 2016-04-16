namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update041 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerAddresses", "Address_City", c => c.String());
            AddColumn("dbo.CustomerAddresses", "Address_Street", c => c.String());
            AddColumn("dbo.CustomerAddresses", "Address_House", c => c.String());
            AddColumn("dbo.CustomerAddresses", "Address_Housing", c => c.String());
            AddColumn("dbo.CustomerAddresses", "Address_Building", c => c.String());
            AddColumn("dbo.CustomerAddresses", "Address_Staircase", c => c.String());
            AddColumn("dbo.CustomerAddresses", "Address_Comment", c => c.String());
            AddColumn("dbo.Drivers", "Address", c => c.String());
            AddColumn("dbo.Drivers", "Email", c => c.String());
            AddColumn("dbo.Drivers", "DriverLicense_Series", c => c.String());
            AddColumn("dbo.Drivers", "DriverLicense_Number", c => c.Int(nullable: false));
            AddColumn("dbo.Drivers", "DriverLicense_DateFrom", c => c.DateTime(nullable: false));
            AddColumn("dbo.Drivers", "DriverLicense_DateTo", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "FromAddress_City", c => c.String());
            AddColumn("dbo.Orders", "FromAddress_Street", c => c.String());
            AddColumn("dbo.Orders", "FromAddress_House", c => c.String());
            AddColumn("dbo.Orders", "FromAddress_Housing", c => c.String());
            AddColumn("dbo.Orders", "FromAddress_Building", c => c.String());
            AddColumn("dbo.Orders", "FromAddress_Staircase", c => c.String());
            AddColumn("dbo.Orders", "FromAddress_Comment", c => c.String());
            AddColumn("dbo.Orders", "ToAddress_City", c => c.String());
            AddColumn("dbo.Orders", "ToAddress_Street", c => c.String());
            AddColumn("dbo.Orders", "ToAddress_House", c => c.String());
            AddColumn("dbo.Orders", "ToAddress_Housing", c => c.String());
            AddColumn("dbo.Orders", "ToAddress_Building", c => c.String());
            AddColumn("dbo.Orders", "ToAddress_Staircase", c => c.String());
            AddColumn("dbo.Orders", "ToAddress_Comment", c => c.String());
            AddColumn("dbo.Orders", "Discount", c => c.Byte(nullable: false));
            AddColumn("dbo.Tariffs", "TariffOptions_Wifi", c => c.Double(nullable: false));
            AddColumn("dbo.Tariffs", "TariffOptions_ChildrenSeat", c => c.Double(nullable: false));
            AddColumn("dbo.Tariffs", "TariffOptions_Conditioner", c => c.Double(nullable: false));
            AddColumn("dbo.Tariffs", "TariffOptions_StationWagon", c => c.Double(nullable: false));
            AddColumn("dbo.Tariffs", "TariffOptions_WithAnimals", c => c.Double(nullable: false));
            AddColumn("dbo.Tariffs", "TariffOptions_Skis", c => c.Double(nullable: false));
            AddColumn("dbo.Tariffs", "TariffOptions_Smoke", c => c.Double(nullable: false));
            AddColumn("dbo.Tariffs", "TariffOptions_Baggage", c => c.Double(nullable: false));
            DropColumn("dbo.CustomerAddresses", "Address");
            DropColumn("dbo.Orders", "FromAddress");
            DropColumn("dbo.Orders", "ToAddress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "ToAddress", c => c.String());
            AddColumn("dbo.Orders", "FromAddress", c => c.String());
            AddColumn("dbo.CustomerAddresses", "Address", c => c.String());
            DropColumn("dbo.Tariffs", "TariffOptions_Baggage");
            DropColumn("dbo.Tariffs", "TariffOptions_Smoke");
            DropColumn("dbo.Tariffs", "TariffOptions_Skis");
            DropColumn("dbo.Tariffs", "TariffOptions_WithAnimals");
            DropColumn("dbo.Tariffs", "TariffOptions_StationWagon");
            DropColumn("dbo.Tariffs", "TariffOptions_Conditioner");
            DropColumn("dbo.Tariffs", "TariffOptions_ChildrenSeat");
            DropColumn("dbo.Tariffs", "TariffOptions_Wifi");
            DropColumn("dbo.Orders", "Discount");
            DropColumn("dbo.Orders", "ToAddress_Comment");
            DropColumn("dbo.Orders", "ToAddress_Staircase");
            DropColumn("dbo.Orders", "ToAddress_Building");
            DropColumn("dbo.Orders", "ToAddress_Housing");
            DropColumn("dbo.Orders", "ToAddress_House");
            DropColumn("dbo.Orders", "ToAddress_Street");
            DropColumn("dbo.Orders", "ToAddress_City");
            DropColumn("dbo.Orders", "FromAddress_Comment");
            DropColumn("dbo.Orders", "FromAddress_Staircase");
            DropColumn("dbo.Orders", "FromAddress_Building");
            DropColumn("dbo.Orders", "FromAddress_Housing");
            DropColumn("dbo.Orders", "FromAddress_House");
            DropColumn("dbo.Orders", "FromAddress_Street");
            DropColumn("dbo.Orders", "FromAddress_City");
            DropColumn("dbo.Drivers", "DriverLicense_DateTo");
            DropColumn("dbo.Drivers", "DriverLicense_DateFrom");
            DropColumn("dbo.Drivers", "DriverLicense_Number");
            DropColumn("dbo.Drivers", "DriverLicense_Series");
            DropColumn("dbo.Drivers", "Email");
            DropColumn("dbo.Drivers", "Address");
            DropColumn("dbo.CustomerAddresses", "Address_Comment");
            DropColumn("dbo.CustomerAddresses", "Address_Staircase");
            DropColumn("dbo.CustomerAddresses", "Address_Building");
            DropColumn("dbo.CustomerAddresses", "Address_Housing");
            DropColumn("dbo.CustomerAddresses", "Address_House");
            DropColumn("dbo.CustomerAddresses", "Address_Street");
            DropColumn("dbo.CustomerAddresses", "Address_City");
        }
    }
}
