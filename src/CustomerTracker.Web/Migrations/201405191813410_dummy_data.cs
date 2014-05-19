namespace CustomerTracker.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dummy_data : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO ROLES (RoleName,IsActive,IsDeleted) VALUES('Admin',1,0)");
            Sql("INSERT INTO ROLES (RoleName,IsActive,IsDeleted) VALUES('Personel',1,0)");
            Sql("INSERT INTO ROLES (RoleName,IsActive,IsDeleted) VALUES('Customer',1,0)");
          
        }
        
        public override void Down()
        {
        }
    }
}
