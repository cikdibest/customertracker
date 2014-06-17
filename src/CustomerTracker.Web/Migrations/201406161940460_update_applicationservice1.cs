namespace CustomerTracker.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_applicationservice1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationServiceRemoteMachines", "ApplicationService_Id", "dbo.ApplicationServices");
            DropForeignKey("dbo.ApplicationServiceRemoteMachines", "RemoteMachine_Id", "dbo.RemoteMachines");
            DropIndex("dbo.ApplicationServiceRemoteMachines", new[] { "ApplicationService_Id" });
            DropIndex("dbo.ApplicationServiceRemoteMachines", new[] { "RemoteMachine_Id" });
            DropTable("dbo.ApplicationServices");
            DropTable("dbo.ApplicationServiceRemoteMachines");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationServiceRemoteMachines",
                c => new
                    {
                        ApplicationService_Id = c.Int(nullable: false),
                        RemoteMachine_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationService_Id, t.RemoteMachine_Id });
            
            CreateTable(
                "dbo.ApplicationServices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstanceName = c.String(maxLength: 50),
                        Description = c.String(maxLength: 1000),
                        ApplicationServiceTypeId = c.Int(nullable: false),
                        CreationDate = c.DateTime(),
                        CreationPersonelId = c.Int(),
                        UpdatedDate = c.DateTime(),
                        UpdatedPersonelId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.ApplicationServiceRemoteMachines", "RemoteMachine_Id");
            CreateIndex("dbo.ApplicationServiceRemoteMachines", "ApplicationService_Id");
            AddForeignKey("dbo.ApplicationServiceRemoteMachines", "RemoteMachine_Id", "dbo.RemoteMachines", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationServiceRemoteMachines", "ApplicationService_Id", "dbo.ApplicationServices", "Id", cascadeDelete: true);
        }
    }
}
