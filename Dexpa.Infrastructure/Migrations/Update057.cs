namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update057 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IpPhoneUsers", "Realm", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.IpPhoneUsers", "Realm");
        }
    }
}
