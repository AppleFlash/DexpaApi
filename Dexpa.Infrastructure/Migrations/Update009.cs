namespace Dexpa.Infrastructure.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Update009 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        Amount = c.Double(nullable: false),
                        Type = c.Int(nullable: false),
                        PaymentMethod = c.Int(nullable: false),
                        Comment = c.String(),
                        Group = c.Int(nullable: false),
                        Driver_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drivers", t => t.Driver_Id)
                .Index(t => t.Driver_Id)
                .Index(t => t.Timestamp);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "Driver_Id", "dbo.Drivers");
            DropIndex("dbo.Transactions", new[] { "Driver_Id" });
            DropIndex("dbo.Transactions", new[] { "Timestamp" });
            DropTable("dbo.Transactions");
        }
    }
}
