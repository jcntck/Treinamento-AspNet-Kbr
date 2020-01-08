using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TreinamentoAspNet02.Areas.Consultor.Controllers
{
    [Authorize(Roles = "Consultor")]
    public class HomeController : Controller
    {
        // GET: Consultor/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}