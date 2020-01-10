namespace OnlineStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v15 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Productoes", newName: "Productos");
            RenameTable(name: "dbo.Ordens", newName: "Ordenes");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Ordenes", newName: "Ordens");
            RenameTable(name: "dbo.Productos", newName: "Productoes");
        }
    }
}
