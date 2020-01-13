using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TreinamentoAspNet02.Models;
using TreinamentoAspNet02.Areas.Admin.Models;
using System.Reflection;

namespace TreinamentoAspNet02.Areas.Admin.Controllers
{
    public class ConsultoresController : Controller
    {
        private ApplicationDbContext Context;
        [HttpGet]
        // GET: Admin/Consultores
        public ActionResult Index()
        {
            ViewBag.StatusMessage = TempData["success"];
            Context = new ApplicationDbContext();
            var users = (from user in Context.Users
                         select new
                         {
                             Id = user.Id,
                             Nome = user.Nome,
                             Email = user.Email,
                             Descricao = user.Descricao,
                             FotoPerfil = user.FotoPerfil,
                             RoleNames = (from userRole in user.Roles
                                          join role in Context.Roles on userRole.RoleId
                                          equals role.Id
                                          select role.Name).ToList()
                         }).Where(x => x.RoleNames.Contains("Consultor")).ToList().Select(p => new Models.IndexViewModel()
                         {
                             Id = p.Id,
                             Nome = p.Nome,
                             Email = p.Email,
                             Descricao = p.Descricao,
                             FotoPerfil = p.FotoPerfil
                         });

            return View(users);
        }
    }
}