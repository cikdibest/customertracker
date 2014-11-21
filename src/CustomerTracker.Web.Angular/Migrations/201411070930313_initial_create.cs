namespace CustomerTracker.Web.Angular.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial_create : DbMigration
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
                        Abbreviation = c.String(maxLength: 20),
                        Keywords = c.String(maxLength: 100),
                        Explanation = c.String(maxLength: 4000),
                        AvatarImageUrl = c.String(maxLength: 250),
                        CityId = c.Int(nullable: false),
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
                        HomePhoneNumber = c.String(maxLength: 100),
                        MobilePhoneNumber = c.String(maxLength: 100),
                        Explanation = c.String(maxLength: 4000),
                        AvatarImageUrl = c.String(maxLength: 250),
                        CustomerId = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
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
                .ForeignKey("dbo.Departments", t => t.DepartmentId)
                .Index(t => t.CustomerId)
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Departments",
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
                "dbo.RemoteMachines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MachineCode = c.String(maxLength: 50),
                        Name = c.String(maxLength: 100),
                        Username = c.String(maxLength: 100),
                        Password = c.String(maxLength: 500),
                        RemoteAddress = c.String(maxLength: 100),
                        Explanation = c.String(maxLength: 4000),
                        CustomerId = c.Int(nullable: false),
                        RemoteMachineConnectionTypeId = c.Int(nullable: false),
                        CreationDate = c.DateTime(),
                        CreationPersonelId = c.Int(),
                        UpdatedDate = c.DateTime(),
                        UpdatedPersonelId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.RemoteMachineConnectionTypes", t => t.RemoteMachineConnectionTypeId)
                .Index(t => t.CustomerId)
                .Index(t => t.RemoteMachineConnectionTypeId);
            
            CreateTable(
                "dbo.RemoteMachineConnectionTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        AvatarImageUrl = c.String(maxLength: 250),
                        CreationDate = c.DateTime(),
                        CreationPersonelId = c.Int(),
                        UpdatedDate = c.DateTime(),
                        UpdatedPersonelId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Explanation = c.String(maxLength: 4000),
                        AvatarImageUrl = c.String(maxLength: 250),
                        Keywords = c.String(maxLength: 100),
                        ParentProductId = c.Int(),
                        CreationDate = c.DateTime(),
                        CreationPersonelId = c.Int(),
                        UpdatedDate = c.DateTime(),
                        UpdatedPersonelId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ParentProductId)
                .Index(t => t.ParentProductId);
            
            CreateTable(
                "dbo.ApplicationServices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstanceName = c.String(maxLength: 50),
                        Description = c.String(maxLength: 1000),
                        ApplicationServiceTypeId = c.Int(nullable: false),
                        RemoteMachineId = c.Int(nullable: false),
                        CreationDate = c.DateTime(),
                        CreationPersonelId = c.Int(),
                        UpdatedDate = c.DateTime(),
                        UpdatedPersonelId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RemoteMachines", t => t.RemoteMachineId)
                .Index(t => t.RemoteMachineId);
            
            CreateTable(
                "dbo.MachineLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MachineConditionJson = c.String(maxLength: 4000),
                        IsAlarm = c.Boolean(nullable: false),
                        RemoteMachineId = c.Int(nullable: false),
                        CreationDate = c.DateTime(),
                        CreationPersonelId = c.Int(),
                        UpdatedDate = c.DateTime(),
                        UpdatedPersonelId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RemoteMachines", t => t.RemoteMachineId)
                .Index(t => t.RemoteMachineId);
            
            CreateTable(
                "dbo.DataMasters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        AvatarImageUrl = c.String(maxLength: 100),
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
                "dbo.DataDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(maxLength: 50),
                        Value = c.String(maxLength: 4000),
                        DataMasterId = c.Int(nullable: false),
                        CreationDate = c.DateTime(),
                        CreationPersonelId = c.Int(),
                        UpdatedDate = c.DateTime(),
                        UpdatedPersonelId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DataMasters", t => t.DataMasterId)
                .Index(t => t.DataMasterId);
            
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
                        Description = c.String(maxLength: 4000),
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
                        Name = c.String(maxLength: 4000),
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
            
            CreateTable(
                "dbo.ProductCustomers",
                c => new
                    {
                        Product_Id = c.Int(nullable: false),
                        Customer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_Id, t.Customer_Id })
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.Customer_Id, cascadeDelete: true)
                .Index(t => t.Product_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.ProductRemoteMachines",
                c => new
                    {
                        Product_Id = c.Int(nullable: false),
                        RemoteMachine_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_Id, t.RemoteMachine_Id })
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .ForeignKey("dbo.RemoteMachines", t => t.RemoteMachine_Id, cascadeDelete: true)
                .Index(t => t.Product_Id)
                .Index(t => t.RemoteMachine_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ProductRemoteMachines", new[] { "RemoteMachine_Id" });
            DropIndex("dbo.ProductRemoteMachines", new[] { "Product_Id" });
            DropIndex("dbo.ProductCustomers", new[] { "Customer_Id" });
            DropIndex("dbo.ProductCustomers", new[] { "Product_Id" });
            DropIndex("dbo.RoleUsers", new[] { "User_Id" });
            DropIndex("dbo.RoleUsers", new[] { "Role_Id" });
            DropIndex("dbo.Solutions", new[] { "SolutionUserId" });
            DropIndex("dbo.Solutions", new[] { "TroubleId" });
            DropIndex("dbo.Solutions", new[] { "ProductId" });
            DropIndex("dbo.Solutions", new[] { "CustomerId" });
            DropIndex("dbo.DataDetails", new[] { "DataMasterId" });
            DropIndex("dbo.DataMasters", new[] { "CustomerId" });
            DropIndex("dbo.MachineLogs", new[] { "RemoteMachineId" });
            DropIndex("dbo.ApplicationServices", new[] { "RemoteMachineId" });
            DropIndex("dbo.Products", new[] { "ParentProductId" });
            DropIndex("dbo.RemoteMachines", new[] { "RemoteMachineConnectionTypeId" });
            DropIndex("dbo.RemoteMachines", new[] { "CustomerId" });
            DropIndex("dbo.Communications", new[] { "DepartmentId" });
            DropIndex("dbo.Communications", new[] { "CustomerId" });
            DropIndex("dbo.Customers", new[] { "CityId" });
            DropIndex("dbo.SocialAccounts", new[] { "UserId" });
            DropForeignKey("dbo.ProductRemoteMachines", "RemoteMachine_Id", "dbo.RemoteMachines");
            DropForeignKey("dbo.ProductRemoteMachines", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.ProductCustomers", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.ProductCustomers", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.RoleUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.RoleUsers", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.Solutions", "SolutionUserId", "dbo.Users");
            DropForeignKey("dbo.Solutions", "TroubleId", "dbo.Troubles");
            DropForeignKey("dbo.Solutions", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Solutions", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.DataDetails", "DataMasterId", "dbo.DataMasters");
            DropForeignKey("dbo.DataMasters", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.MachineLogs", "RemoteMachineId", "dbo.RemoteMachines");
            DropForeignKey("dbo.ApplicationServices", "RemoteMachineId", "dbo.RemoteMachines");
            DropForeignKey("dbo.Products", "ParentProductId", "dbo.Products");
            DropForeignKey("dbo.RemoteMachines", "RemoteMachineConnectionTypeId", "dbo.RemoteMachineConnectionTypes");
            DropForeignKey("dbo.RemoteMachines", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Communications", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Communications", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Customers", "CityId", "dbo.Cities");
            DropForeignKey("dbo.SocialAccounts", "UserId", "dbo.Users");
            DropTable("dbo.ProductRemoteMachines");
            DropTable("dbo.ProductCustomers");
            DropTable("dbo.RoleUsers");
            DropTable("dbo.Troubles");
            DropTable("dbo.Solutions");
            DropTable("dbo.DataDetails");
            DropTable("dbo.DataMasters");
            DropTable("dbo.MachineLogs");
            DropTable("dbo.ApplicationServices");
            DropTable("dbo.Products");
            DropTable("dbo.RemoteMachineConnectionTypes");
            DropTable("dbo.RemoteMachines");
            DropTable("dbo.Departments");
            DropTable("dbo.Communications");
            DropTable("dbo.Cities");
            DropTable("dbo.Customers");
            DropTable("dbo.SocialAccounts");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
        }
    }
}
