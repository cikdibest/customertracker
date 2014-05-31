namespace CustomerTracker.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifty_entities : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "LogoImageName", c => c.String(maxLength: 100));
            AddColumn("dbo.Communications", "LogoImageName", c => c.String(maxLength: 100));
            AddColumn("dbo.Products", "LogoImageName", c => c.String(maxLength: 100));
            AddColumn("dbo.DataMasters", "LogoImageName", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DataMasters", "LogoImageName");
            DropColumn("dbo.Products", "LogoImageName");
            DropColumn("dbo.Communications", "LogoImageName");
            DropColumn("dbo.Customers", "LogoImageName");
        }
    }
}
