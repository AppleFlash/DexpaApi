namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update068 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "BelongsCompany", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cars", "BelongsCompany");
        }
    }
}
