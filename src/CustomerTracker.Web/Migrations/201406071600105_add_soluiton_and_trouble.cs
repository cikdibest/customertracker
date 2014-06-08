namespace CustomerTracker.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_soluiton_and_trouble : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Solutions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        TroubleId = c.Int(nullable: false),
                        SolutionUserId = c.Int(nullable: false),
                        Title = c.String(maxLength: 250),
                        Description = c.String(),
                        CreationDate = c.DateTime(),
                        CreationPersonelId = c.Int(),
                        UpdatedDate = c.DateTime(),
                        UpdatedPersonelId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .ForeignKey("dbo.Troubles", t => t.TroubleId)
                .ForeignKey("dbo.Users", t => t.SolutionUserId)
                .Index(t => t.CustomerId)
                .Index(t => t.ProductId)
                .Index(t => t.TroubleId)
                .Index(t => t.SolutionUserId);
            
            CreateTable(
                "dbo.Troubles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreationDate = c.DateTime(),
                        CreationPersonelId = c.Int(),
                        UpdatedDate = c.DateTime(),
                        UpdatedPersonelId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Solutions", new[] { "SolutionUserId" });
            DropIndex("dbo.Solutions", new[] { "TroubleId" });
            DropIndex("dbo.Solutions", new[] { "ProductId" });
            DropIndex("dbo.Solutions", new[] { "CustomerId" });
            DropForeignKey("dbo.Solutions", "SolutionUserId", "dbo.Users");
            DropForeignKey("dbo.Solutions", "TroubleId", "dbo.Troubles");
            DropForeignKey("dbo.Solutions", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Solutions", "CustomerId", "dbo.Customers");
            DropTable("dbo.Troubles");
            DropTable("dbo.Solutions");
        }
    }
}
