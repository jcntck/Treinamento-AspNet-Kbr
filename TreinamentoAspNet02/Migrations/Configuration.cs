namespace TreinamentoAspNet02.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TreinamentoAspNet02.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TreinamentoAspNet02.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "TreinamentoAspNet02.Context";
        }

        protected override void Seed(TreinamentoAspNet02.Models.ApplicationDbContext context)
        {
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var manager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "admin@email.com"))
            {
                var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
                var user = new ApplicationUser { UserName = "admin@email.com", Email = "admin@email.com", Active = true, Nome = "Administrador", Descricao = "Administrador do Sistema" };

                manager.Create(user, "Admin@123");
                manager.AddToRole(user.Id, "Admin");
            }
        }
    }
}
