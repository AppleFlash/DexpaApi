namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update012 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "Balance", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Drivers", "Balance");
        }
    }
}
