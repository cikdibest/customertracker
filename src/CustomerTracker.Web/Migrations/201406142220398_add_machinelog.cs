namespace CustomerTracker.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_machinelog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MachineLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MachineConditionJson = c.String(),
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
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.MachineLogs", new[] { "RemoteMachineId" });
            DropForeignKey("dbo.MachineLogs", "RemoteMachineId", "dbo.RemoteMachines");
            DropTable("dbo.MachineLogs");
        }
    }
}
