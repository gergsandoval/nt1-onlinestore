namespace OnlineStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CarritoItems", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.CarritoItems", new[] { "User_Id" });
            AddColumn("dbo.CarritoItems", "UserEmail", c => c.String());
            DropColumn("dbo.CarritoItems", "UserId");
            DropColumn("dbo.CarritoItems", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CarritoItems", "User_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.CarritoItems", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.CarritoItems", "UserEmail");
            CreateIndex("dbo.CarritoItems", "User_Id");
            AddForeignKey("dbo.CarritoItems", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
