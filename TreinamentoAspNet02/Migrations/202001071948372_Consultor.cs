namespace TreinamentoAspNet02.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Consultor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Nome", c => c.String());
            AddColumn("dbo.AspNetUsers", "Descricao", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Descricao");
            DropColumn("dbo.AspNetUsers", "Nome");
        }
    }
}
