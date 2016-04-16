namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update055 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GlobalSettings", "TechnicalSupportFeeSize", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GlobalSettings", "TechnicalSupportFeeSize");
        }
    }
}
