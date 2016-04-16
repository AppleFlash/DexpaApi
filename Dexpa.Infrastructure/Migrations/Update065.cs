namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update065 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GlobalSettings", "RentTransactionTime", c => c.String());
            AlterColumn("dbo.GlobalSettings", "BalanceRecalculateTimeInterval", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GlobalSettings", "BalanceRecalculateTimeInterval", c => c.Int(nullable: false));
            AlterColumn("dbo.GlobalSettings", "RentTransactionTime", c => c.Int(nullable: false));
        }
    }
}
