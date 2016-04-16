namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update016 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drivers", "BalanceLimit", c => c.Double(nullable: false));
            AddColumn("dbo.Drivers", "DayTimeFee", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Drivers", "DayTimeFee");
            DropColumn("dbo.Drivers", "BalanceLimit");
        }
    }
}
