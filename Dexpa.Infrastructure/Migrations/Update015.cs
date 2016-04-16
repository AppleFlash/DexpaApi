namespace Dexpa.Infrastructure.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Update015 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Driver_Id", c => c.Long());
            CreateIndex("dbo.AspNetUsers", "Driver_Id");
            AddForeignKey("dbo.AspNetUsers", "Driver_Id", "dbo.Drivers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Driver_Id", "dbo.Drivers");
            DropIndex("dbo.AspNetUsers", new[] { "Driver_Id" });
            DropColumn("dbo.AspNetUsers", "Driver_Id");
        }
    }
}
