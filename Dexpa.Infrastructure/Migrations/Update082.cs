namespace Dexpa.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update082 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewsMessages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Message = c.String(maxLength: 512),
                        IsSend = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        AuthorLogin = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NewsMessages");
        }
    }
}
