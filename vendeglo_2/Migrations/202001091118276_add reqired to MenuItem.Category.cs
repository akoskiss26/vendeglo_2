namespace vendeglo_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addreqiredtoMenuItemCategory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MenuItems", "Category_Id", "dbo.Categories");
            DropIndex("dbo.MenuItems", new[] { "Category_Id" });
            AlterColumn("dbo.MenuItems", "Category_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.MenuItems", "Category_Id");
            AddForeignKey("dbo.MenuItems", "Category_Id", "dbo.Categories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MenuItems", "Category_Id", "dbo.Categories");
            DropIndex("dbo.MenuItems", new[] { "Category_Id" });
            AlterColumn("dbo.MenuItems", "Category_Id", c => c.Int());
            CreateIndex("dbo.MenuItems", "Category_Id");
            AddForeignKey("dbo.MenuItems", "Category_Id", "dbo.Categories", "Id");
        }
    }
}
