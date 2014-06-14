namespace CustomerTracker.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_applicationservice_name : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationServices", "InstanceName", c => c.String());
            Sql("UPDATE dbo.ApplicationServices SET InstanceName=Name ");
            DropColumn("dbo.ApplicationServices", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationServices", "Name", c => c.String());
            DropColumn("dbo.ApplicationServices", "InstanceName");
        }
    }
}
