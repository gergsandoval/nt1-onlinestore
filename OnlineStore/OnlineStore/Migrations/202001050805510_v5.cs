namespace OnlineStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CarritoItems", "Producto_ProductoId", "dbo.Productoes");
            DropIndex("dbo.CarritoItems", new[] { "Producto_ProductoId" });
            RenameColumn(table: "dbo.CarritoItems", name: "Producto_ProductoId", newName: "ProductoId");
            AddColumn("dbo.CarritoItems", "Subtotal", c => c.Int(nullable: false));
            AlterColumn("dbo.CarritoItems", "ProductoId", c => c.Int(nullable: false));
            CreateIndex("dbo.CarritoItems", "ProductoId");
            AddForeignKey("dbo.CarritoItems", "ProductoId", "dbo.Productoes", "ProductoId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CarritoItems", "ProductoId", "dbo.Productoes");
            DropIndex("dbo.CarritoItems", new[] { "ProductoId" });
            AlterColumn("dbo.CarritoItems", "ProductoId", c => c.Int());
            DropColumn("dbo.CarritoItems", "Subtotal");
            RenameColumn(table: "dbo.CarritoItems", name: "ProductoId", newName: "Producto_ProductoId");
            CreateIndex("dbo.CarritoItems", "Producto_ProductoId");
            AddForeignKey("dbo.CarritoItems", "Producto_ProductoId", "dbo.Productoes", "ProductoId");
        }
    }
}
