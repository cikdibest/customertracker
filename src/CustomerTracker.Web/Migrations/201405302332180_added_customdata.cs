namespace CustomerTracker.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_customdata : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DataMasters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        CreationDate = c.DateTime(),
                        CreationPersonelId = c.Int(),
                        UpdatedDate = c.DateTime(),
                        UpdatedPersonelId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DataDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(maxLength: 50),
                        Value = c.String(maxLength: 4000),
                        DataMasterId = c.Int(nullable: false),
                        CreationDate = c.DateTime(),
                        CreationPersonelId = c.Int(),
                        UpdatedDate = c.DateTime(),
                        UpdatedPersonelId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DataMasters", t => t.DataMasterId)
                .Index(t => t.DataMasterId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.DataDetails", new[] { "DataMasterId" });
            DropForeignKey("dbo.DataDetails", "DataMasterId", "dbo.DataMasters");
            DropTable("dbo.DataDetails");
            DropTable("dbo.DataMasters");
        }
    }
}
