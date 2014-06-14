namespace CustomerTracker.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_applicationservice : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationServiceProducts", "ApplicationService_Id", "dbo.ApplicationServices");
            DropForeignKey("dbo.ApplicationServiceProducts", "Product_Id", "dbo.Products");
            DropIndex("dbo.ApplicationServiceProducts", new[] { "ApplicationService_Id" });
            DropIndex("dbo.ApplicationServiceProducts", new[] { "Product_Id" });
            CreateTable(
                "dbo.ApplicationServiceRemoteMachines",
                c => new
                    {
                        ApplicationService_Id = c.Int(nullable: false),
                        RemoteMachine_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationService_Id, t.RemoteMachine_Id })
                .ForeignKey("dbo.ApplicationServices", t => t.ApplicationService_Id, cascadeDelete: true)
                .ForeignKey("dbo.RemoteMachines", t => t.RemoteMachine_Id, cascadeDelete: true)
                .Index(t => t.ApplicationService_Id)
                .Index(t => t.RemoteMachine_Id);
            
            AddColumn("dbo.RemoteMachines", "MachineCode", c => c.String(maxLength: 10));
            AddColumn("dbo.ApplicationServices", "ApplicationServiceTypeId", c => c.Int(nullable: false));
            DropTable("dbo.ApplicationServiceProducts");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationServiceProducts",
                c => new
                    {
                        ApplicationService_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationService_Id, t.Product_Id });
            
            DropIndex("dbo.ApplicationServiceRemoteMachines", new[] { "RemoteMachine_Id" });
            DropIndex("dbo.ApplicationServiceRemoteMachines", new[] { "ApplicationService_Id" });
            DropForeignKey("dbo.ApplicationServiceRemoteMachines", "RemoteMachine_Id", "dbo.RemoteMachines");
            DropForeignKey("dbo.ApplicationServiceRemoteMachines", "ApplicationService_Id", "dbo.ApplicationServices");
            DropColumn("dbo.ApplicationServices", "ApplicationServiceTypeId");
            DropColumn("dbo.RemoteMachines", "MachineCode");
            DropTable("dbo.ApplicationServiceRemoteMachines");
            CreateIndex("dbo.ApplicationServiceProducts", "Product_Id");
            CreateIndex("dbo.ApplicationServiceProducts", "ApplicationService_Id");
            AddForeignKey("dbo.ApplicationServiceProducts", "Product_Id", "dbo.Products", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationServiceProducts", "ApplicationService_Id", "dbo.ApplicationServices", "Id", cascadeDelete: true);
        }
    }
}
