namespace CustomerTracker.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_explanationcolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Communications", "Explanation", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Communications", "Explanation");
        }
    }
}
