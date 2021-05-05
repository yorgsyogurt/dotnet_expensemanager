namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Operation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operations", "name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Operations", "name");
        }
    }
}
