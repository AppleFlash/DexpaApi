namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update056 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IpPhoneUsers",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Login = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.IpPhoneUsers");
        }
    }
}
