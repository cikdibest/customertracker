namespace CustomerTracker.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_remotemachine_machincode1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RemoteMachines", "MachineCode", c => c.String(maxLength: 15));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RemoteMachines", "MachineCode", c => c.String(maxLength: 50));
        }
    }
}
