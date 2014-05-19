namespace CustomerTracker.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class frstmigartion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 100),
                        Email = c.String(maxLength: 100),
                        Password = c.String(nullable: false, maxLength: 100),
                        FirstName = c.String(maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        IsApproved = c.Boolean(nullable: false),
                        PasswordFailuresSinceLastSuccess = c.Int(nullable: false),
                        LastPasswordFailureDate = c.DateTime(),
                        LastActivityDate = c.DateTime(),
                        LastLockoutDate = c.DateTime(),
                        LastLoginDate = c.DateTime(),
                        ConfirmationToken = c.String(maxLength: 100),
                        IsLockedOut = c.Boolean(nullable: false),
                        LastPasswordChangedDate = c.DateTime(),
                        PasswordVerificationToken = c.String(maxLength: 100),
                        PasswordVerificationTokenExpirationDate = c.DateTime(),
                        CreationDate = c.DateTime(),
                        CreationPersonelId = c.Int(),
                        UpdatedDate = c.DateTime(),
                        UpdatedPersonelId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 50),
                        CreationDate = c.DateTime(),
                        CreationPersonelId = c.Int(),
                        UpdatedDate = c.DateTime(),
                        UpdatedPersonelId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SocialAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Provider = c.String(maxLength: 100),
                        ProviderUserId = c.String(maxLength: 500),
                        CreationDate = c.DateTime(),
                        CreationPersonelId = c.Int(),
                        UpdatedDate = c.DateTime(),
                        UpdatedPersonelId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        CityId = c.Int(nullable: false),
                        Explanation = c.String(maxLength: 4000),
                        CreationDate = c.DateTime(),
                        CreationPersonelId = c.Int(),
                        UpdatedDate = c.DateTime(),
                        UpdatedPersonelId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Code = c.String(maxLength: 10),
                        CreationDate = c.DateTime(),
                        CreationPersonelId = c.Int(),
                        UpdatedDate = c.DateTime(),
                        UpdatedPersonelId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Communications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        Email = c.String(maxLength: 100),
                        PhoneNumber = c.String(maxLength: 100),
                        CustomerId = c.Int(nullable: false),
                        TitleId = c.Int(nullable: false),
                        GenderId = c.Int(nullable: false),
                        CreationDate = c.DateTime(),
                        CreationPersonelId = c.Int(),
                        UpdatedDate = c.DateTime(),
                        UpdatedPersonelId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Titles", t => t.TitleId)
                .Index(t => t.CustomerId)
                .Index(t => t.TitleId);
            
            CreateTable(
                "dbo.Titles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        CreationDate = c.DateTime(),
                        CreationPersonelId = c.Int(),
                        UpdatedDate = c.DateTime(),
                        UpdatedPersonelId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RemoteComputers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Username = c.String(maxLength: 100),
                        Password = c.String(maxLength: 500),
                        Explanation = c.String(maxLength: 4000),
                        RemoteAddress = c.String(maxLength: 100),
                        RemoteConnectionTypeId = c.Int(nullable: false),
                        ProductTags = c.String(maxLength: 200),
                        CustomerId = c.Int(nullable: false),
                        CreationDate = c.DateTime(),
                        CreationPersonelId = c.Int(),
                        UpdatedDate = c.DateTime(),
                        UpdatedPersonelId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Explanation = c.String(maxLength: 4000),
                        CreationDate = c.DateTime(),
                        CreationPersonelId = c.Int(),
                        UpdatedDate = c.DateTime(),
                        UpdatedPersonelId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleUsers",
                c => new
                    {
                        Role_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.User_Id })
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.RoleUsers", new[] { "User_Id" });
            DropIndex("dbo.RoleUsers", new[] { "Role_Id" });
            DropIndex("dbo.RemoteComputers", new[] { "CustomerId" });
            DropIndex("dbo.Communications", new[] { "TitleId" });
            DropIndex("dbo.Communications", new[] { "CustomerId" });
            DropIndex("dbo.Customers", new[] { "CityId" });
            DropIndex("dbo.SocialAccounts", new[] { "UserId" });
            DropForeignKey("dbo.RoleUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.RoleUsers", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.RemoteComputers", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Communications", "TitleId", "dbo.Titles");
            DropForeignKey("dbo.Communications", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Customers", "CityId", "dbo.Cities");
            DropForeignKey("dbo.SocialAccounts", "UserId", "dbo.Users");
            DropTable("dbo.RoleUsers");
            DropTable("dbo.Products");
            DropTable("dbo.RemoteComputers");
            DropTable("dbo.Titles");
            DropTable("dbo.Communications");
            DropTable("dbo.Cities");
            DropTable("dbo.Customers");
            DropTable("dbo.SocialAccounts");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
        }
    }
}
