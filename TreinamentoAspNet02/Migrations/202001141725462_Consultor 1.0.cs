namespace TreinamentoAspNet02.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Consultor10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "AutoId", c => c.Int(nullable: false, identity: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "AutoId");
        }
    }
}
