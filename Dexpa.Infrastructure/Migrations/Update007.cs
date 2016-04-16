namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update0007 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "Timestamp", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cars", "Timestamp");
        }
    }
}
