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
                    Celular = model.Celular
                };

                db.Visitante.Add(visitante);
                db.SaveChanges();

                int idAtendimento = CriarNovoAtendimento(visitante.Id, model.IdConsultor, model.TempoAtendimento.Duracao);

                return RedirectToAction("Chat", "Home", new { id = idAtendimento });
            }
            // Se chegamos até aqui e houver alguma falha, exiba novamente o formulário
            return RedirectToAction("Create", "Visitante", new { consultorId = model.IdConsultor });
        }

        public int CriarNovoAtendimento(int idVisitante, string idConsultor, int duracao)
        {
            var atendimento = new Entity.Atendimentos
            {
                Id_Visitante = idVisitante,
                Id_Consultor = idConsultor,
                Data = DateTime.Now,
                Duracao = duracao
            };

            db.Atendimentos.Add(atendimento);
            db.SaveChanges();
            return atendimento.Id;
        }
    }
}