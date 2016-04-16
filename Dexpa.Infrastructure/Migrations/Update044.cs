namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update044 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DriverPassword", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DriverPassword");
        }
    }
}
