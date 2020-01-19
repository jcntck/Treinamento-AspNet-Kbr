using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TreinamentoAspNet02.Entity;

namespace TreinamentoAspNet02.Areas.Consultor.Controllers
{
    [Authorize(Roles = "Consultor")]
    public class HomeController : Controller
    {
        private sistema_atendimentoEntities db = new sistema_atendimentoEntities();
        // GET: Consultor/Home
        public ActionResult Index()
        {
            return View();
        }

        public void TrocarStatus()
        {
            var user = db.AspNetUsers.First(x => x.UserName == User.Identity.Name);

            user.Ocupado = user.Ocupado == false ? true : false;

            var result = db.SaveChanges();
        }

        public JsonResult GetStatus()
        {
            var user = db.AspNetUsers.First(x => x.UserName == User.Identity.Name);
            return Json(user.Ocupado);
        }
    }
}