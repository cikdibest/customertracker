namespace CustomerTracker.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class logoimagename : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "LogoImageUrl", c => c.String(maxLength: 100));
            AddColumn("dbo.Communications", "LogoImageUrl", c => c.String(maxLength: 100));
            AddColumn("dbo.Products", "LogoImageUrl", c => c.String(maxLength: 100));
            AddColumn("dbo.DataMasters", "LogoImageUrl", c => c.String(maxLength: 100));
            AddColumn("dbo.DataDetails", "IsRequiredEncrypted", c => c.Boolean(nullable: false));
            DropColumn("dbo.Customers", "LogoImageName");
            DropColumn("dbo.Communications", "LogoImageName");
            DropColumn("dbo.Products", "LogoImageName");
            DropColumn("dbo.DataMasters", "LogoImageName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DataMasters", "LogoImageName", c => c.String(maxLength: 100));
            AddColumn("dbo.Products", "LogoImageName", c => c.String(maxLength: 100));
            AddColumn("dbo.Communications", "LogoImageName", c => c.String(maxLength: 100));
            AddColumn("dbo.Customers", "LogoImageName", c => c.String(maxLength: 100));
            DropColumn("dbo.DataDetails", "IsRequiredEncrypted");
            DropColumn("dbo.DataMasters", "LogoImageUrl");
            DropColumn("dbo.Products", "LogoImageUrl");
            DropColumn("dbo.Communications", "LogoImageUrl");
            DropColumn("dbo.Customers", "LogoImageUrl");
        }
    }
}
