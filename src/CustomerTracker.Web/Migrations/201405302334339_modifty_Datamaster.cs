namespace CustomerTracker.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifty_Datamaster : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DataMasters", "CustomerId", c => c.Int(nullable: false));
            AddForeignKey("dbo.DataMasters", "CustomerId", "dbo.Customers", "Id");
            CreateIndex("dbo.DataMasters", "CustomerId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.DataMasters", new[] { "CustomerId" });
            DropForeignKey("dbo.DataMasters", "CustomerId", "dbo.Customers");
            DropColumn("dbo.DataMasters", "CustomerId");
        }
    }
}
