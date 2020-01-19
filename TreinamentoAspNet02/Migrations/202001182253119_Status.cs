namespace TreinamentoAspNet02.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Status : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Ocupado", c => c.Boolean(nullable: false, defaultValueSql: "1"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Ocupado");
        }
    }
}
