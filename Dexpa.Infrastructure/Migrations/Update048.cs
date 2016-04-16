namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update048 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cars", "Permission_Number", c => c.String());
            AlterColumn("dbo.Cars", "Permission_Number2", c => c.String());
            AlterColumn("dbo.Drivers", "DriverLicense_Number", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Drivers", "DriverLicense_Number", c => c.Int(nullable: false));
            AlterColumn("dbo.Cars", "Permission_Number2", c => c.Int(nullable: false));
            AlterColumn("dbo.Cars", "Permission_Number", c => c.Int(nullable: false));
        }
    }
}
