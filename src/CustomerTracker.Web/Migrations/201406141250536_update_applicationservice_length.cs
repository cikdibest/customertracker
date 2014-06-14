namespace CustomerTracker.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_applicationservice_length : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ApplicationServices", "InstanceName", c => c.String(maxLength: 50));
            AlterColumn("dbo.ApplicationServices", "Description", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ApplicationServices", "Description", c => c.String());
            AlterColumn("dbo.ApplicationServices", "InstanceName", c => c.String());
        }
    }
}
