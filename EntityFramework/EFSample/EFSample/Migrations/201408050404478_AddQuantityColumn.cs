namespace EFSample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQuantityColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TFSUsers", "Quantity", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TFSUsers", "Quantity");
        }
    }
}
