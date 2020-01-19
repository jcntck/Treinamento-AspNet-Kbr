using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TreinamentoAspNet02.Chat;
using TreinamentoAspNet02.Entity;
using TreinamentoAspNet02.Models;

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

        [HttpPost]
        public JsonResult Conectados(List<UserDetail> connectedUsers)
        {
            List<Consultores> consultores = new List<Consultores>();
            foreach (UserDetail connectedUser in connectedUsers)
            { 
                var user = db.AspNetUsers.FirstOrDefault(u => u.UserName == connectedUser.UserName);
                consultores.Add(new Consultores
                {
                    Id = user.Id,
                    Nome = user.Nome,
                    Email = user.Email,
                    UserName = user.UserName,
                    Descricao = user.Descricao,
                    FotoPerfil = user.FotoPerfil,
                    Ocupado = user.Ocupado
                });
            }

            var jsonSerializer = new JavaScriptSerializer();
            var json = jsonSerializer.Serialize(consultores);
            return Json(consultores);
        }
    }
}