namespace EFSample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TFSUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserAgent = c.String(),
                        UserName = c.String(),
                        CommandDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TFSUsers");
        }
    }
}
