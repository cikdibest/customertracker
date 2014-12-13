namespace CustomerTracker.Web.Angular.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_datamaster : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DataMasters", "CustomerId", "dbo.Customers");
            DropIndex("dbo.DataMasters", new[] { "CustomerId" });
            CreateTable(
                "dbo.DataMasterUsers",
                c => new
                    {
                        DataMaster_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DataMaster_Id, t.User_Id })
                .ForeignKey("dbo.DataMasters", t => t.DataMaster_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.DataMaster_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.CustomerDataMasters",
                c => new
                    {
                        Customer_Id = c.Int(nullable: false),
                        DataMaster_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Customer_Id, t.DataMaster_Id })
                .ForeignKey("dbo.Customers", t => t.Customer_Id, cascadeDelete: true)
                .ForeignKey("dbo.DataMasters", t => t.DataMaster_Id, cascadeDelete: true)
                .Index(t => t.Customer_Id)
                .Index(t => t.DataMaster_Id);
            
            DropColumn("dbo.DataMasters", "CustomerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DataMasters", "CustomerId", c => c.Int(nullable: false));
            DropIndex("dbo.CustomerDataMasters", new[] { "DataMaster_Id" });
            DropIndex("dbo.CustomerDataMasters", new[] { "Customer_Id" });
            DropIndex("dbo.DataMasterUsers", new[] { "User_Id" });
            DropIndex("dbo.DataMasterUsers", new[] { "DataMaster_Id" });
            DropForeignKey("dbo.CustomerDataMasters", "DataMaster_Id", "dbo.DataMasters");
            DropForeignKey("dbo.CustomerDataMasters", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.DataMasterUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.DataMasterUsers", "DataMaster_Id", "dbo.DataMasters");
            DropTable("dbo.CustomerDataMasters");
            DropTable("dbo.DataMasterUsers");
            CreateIndex("dbo.DataMasters", "CustomerId");
            AddForeignKey("dbo.DataMasters", "CustomerId", "dbo.Customers", "Id");
        }
    }
}
