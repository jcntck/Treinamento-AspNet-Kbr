using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TreinamentoAspNet02.Areas.Admin.Controllers
{
    public class AtendimentosController : Controller
    {
        // GET: Admin/Atendimentos
        public ActionResult Index()
        {
            return View();
        }
    }
}