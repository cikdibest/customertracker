namespace CustomerTracker.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_applicationservice : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationServices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CreationDate = c.DateTime(),
                        CreationPersonelId = c.Int(),
                        UpdatedDate = c.DateTime(),
                        UpdatedPersonelId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationServiceProducts",
                c => new
                    {
                        ApplicationService_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationService_Id, t.Product_Id })
                .ForeignKey("dbo.ApplicationServices", t => t.ApplicationService_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.ApplicationService_Id)
                .Index(t => t.Product_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ApplicationServiceProducts", new[] { "Product_Id" });
            DropIndex("dbo.ApplicationServiceProducts", new[] { "ApplicationService_Id" });
            DropForeignKey("dbo.ApplicationServiceProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.ApplicationServiceProducts", "ApplicationService_Id", "dbo.ApplicationServices");
            DropTable("dbo.ApplicationServiceProducts");
            DropTable("dbo.ApplicationServices");
        }
    }
}
