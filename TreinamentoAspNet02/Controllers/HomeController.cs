using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TreinamentoAspNet02.Entity;

namespace TreinamentoAspNet02.Controllers
{
    public class HomeController : Controller
    {
        private sistema_atendimentoEntities db = new sistema_atendimentoEntities();
        public ActionResult Index()
        {
            var users = db.AspNetRoles.Where(s => s.Name == "Consultor").FirstOrDefault().AspNetUsers.OrderByDescending(s => s.AutoId).ToList();
            return View(users);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Mais sobre esse sistema.";

            return View();
        }

        public ActionResult Chat(string id)
        {
            var user = db.AspNetUsers.Find(id);
            return View(user);
        }
    }
}