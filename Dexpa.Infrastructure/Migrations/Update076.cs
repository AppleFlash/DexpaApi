namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update076 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contents", "WebUrlThumb", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contents", "WebUrlThumb");
        }
    }
}
