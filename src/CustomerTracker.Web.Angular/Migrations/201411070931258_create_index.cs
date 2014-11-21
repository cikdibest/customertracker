namespace CustomerTracker.Web.Angular.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_index : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.RemoteMachines", "MachineCode", true, "IX_MachineCode");
        }
        
        public override void Down()
        {
        }
    }
}
