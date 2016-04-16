namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update024 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tariffs", "IncludeMinutesAndKilometers", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tariffs", "IncludeMinutesAndKilometers");
        }
    }
}
