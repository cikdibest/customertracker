namespace CustomerTracker.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_index_on_remotemachine : DbMigration
    {
        public override void Up()
        {
            string machineCode = "'C' + CONVERT(nvarchar,CustomerId) + 'R' + CONVERT(nvarchar,Id) + 'T' + CONVERT(nvarchar, DATEPART(HOUR, GETDATE()) + DATEPART(dd, GETDATE()) + DATEPART(MM, GETDATE()))";
            Sql("UPDATE dbo.RemoteMachines SET MachineCode= "+ machineCode);
            CreateIndex("dbo.RemoteMachines","MachineCode",true,"IX_MachineCode");
        }
        
        public override void Down()
        {
        }
    }
}
