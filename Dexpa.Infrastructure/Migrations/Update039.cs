namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update039 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "OrderOptions_CarFeatures", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "OrderOptions_ChildrenSeat", c => c.Int(nullable: false));
            AddColumn("dbo.Tariffs", "YandexId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tariffs", "YandexId");
            DropColumn("dbo.Orders", "OrderOptions_ChildrenSeat");
            DropColumn("dbo.Orders", "OrderOptions_CarFeatures");
        }
    }
}
