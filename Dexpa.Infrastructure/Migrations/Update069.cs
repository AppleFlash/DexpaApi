namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update069 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "PrivatePhone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "PrivatePhone");
        }
    }
}
