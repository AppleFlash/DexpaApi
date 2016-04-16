namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update033 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tariffs", "Uuid", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tariffs", "Uuid");
        }
    }
}
