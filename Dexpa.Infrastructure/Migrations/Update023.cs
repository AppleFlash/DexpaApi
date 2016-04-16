namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update023 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tariffs", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tariffs", "Comment");
        }
    }
}
