namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update049 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "TechnicalSupport", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Drivers", "TechnicalSupport");
        }
    }
}
