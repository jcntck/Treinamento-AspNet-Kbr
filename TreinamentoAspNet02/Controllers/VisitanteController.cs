using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TreinamentoAspNet02.Entity;
using TreinamentoAspNet02.Models;

namespace TreinamentoAspNet02.Controllers
{
    public class VisitanteController : Controller
    {
        private sistema_atendimentoEntities db = new sistema_atendimentoEntities();
        // GET: Visitante/Create
        public ActionResult Create(string consultorId)
        {
            if (!string.IsNullOrEmpty(consultorId))
            {
                var user = db.AspNetUsers.Find(consultorId);

                ViewBag.IdConsultor = user.Id;
                ViewBag.NomeConsultor = user.Nome;

                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Visitante/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var visitante = new Entity.Visitante
                {
                    Nome = model.Nome,
                    Email = model.Email,
                    Celular = model.Celular,
                    Id_Consultor = model.IdConsultor
                };

                db.Visitante.Add(visitante);
                db.SaveChanges();
                int id = visitante.Id;
                return RedirectToAction("Chat", "Home", new { id = model.IdConsultor });
            }
            // Se chegamos até aqui e houver alguma falha, exiba novamente o formulário
            return View(model);
        }
    }
}