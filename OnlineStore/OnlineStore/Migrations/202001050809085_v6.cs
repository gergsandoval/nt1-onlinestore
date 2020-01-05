namespace OnlineStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v6 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CarritoItems", "Subtotal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CarritoItems", "Subtotal", c => c.Int(nullable: false));
        }
    }
}
