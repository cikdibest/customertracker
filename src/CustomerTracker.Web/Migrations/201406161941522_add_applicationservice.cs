namespace CustomerTracker.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_applicationservice : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ApplicationServices", new[] { "RemoteMachineId" });
            DropForeignKey("dbo.ApplicationServices", "RemoteMachineId", "dbo.RemoteMachines");
            DropTable("dbo.ApplicationServices");
        }
    }
}
