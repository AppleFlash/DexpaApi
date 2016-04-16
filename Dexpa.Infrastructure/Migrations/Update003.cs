namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update003 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "State", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Drivers", "State");
        }
    }
}
