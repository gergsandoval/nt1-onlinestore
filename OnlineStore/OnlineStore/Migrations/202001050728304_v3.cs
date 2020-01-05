namespace OnlineStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CarritoItems",
                c => new
                    {
                        CarritoItemId = c.Int(nullable: false, identity: true),
                        Cantidad = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Producto_ProductoId = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CarritoItemId)
                .ForeignKey("dbo.Productoes", t => t.Producto_ProductoId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Producto_ProductoId)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CarritoItems", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CarritoItems", "Producto_ProductoId", "dbo.Productoes");
            DropIndex("dbo.CarritoItems", new[] { "User_Id" });
            DropIndex("dbo.CarritoItems", new[] { "Producto_ProductoId" });
            DropTable("dbo.CarritoItems");
        }
    }
}
