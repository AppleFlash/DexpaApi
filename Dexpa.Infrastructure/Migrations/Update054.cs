namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update054 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "InsertDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "InsertDate");
        }
    }
}
