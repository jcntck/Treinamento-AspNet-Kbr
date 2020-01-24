using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TreinamentoAspNet02.Areas.Admin.Models;
using TreinamentoAspNet02.Entity;

namespace TreinamentoAspNet02.Areas.Consultor.Controllers
{
    [Authorize(Roles = "Consultor")]
    public class AtendimentosController : Controller
    {
        private sistema_atendimentoEntities1 db = new sistema_atendimentoEntities1();

        // GET: Consultor/Atendimentos
        public ActionResult Index()
        {
            var atendimentos = (from consultor in db.AspNetUsers
                                join atendimento in db.Atendimentos on consultor.Id equals atendimento.Id_Consultor
                                join visitante in db.Visitante on atendimento.Id_Visitante equals visitante.Id
                                where consultor.UserName == User.Identity.Name
                                orderby atendimento.Id descending
                                select new
                                {
                                    Id = atendimento.Id,
                                    NomeConsultor = consultor.Nome,
                                    NomeCliente = visitante.Nome,
                                    Data = atendimento.Data
                                }).ToList().Select(a => new AtendimentosViewModel
                                {
                                    Id = a.Id,
                                    NomeConsultor = a.NomeConsultor,
                                    NomeCliente = a.NomeCliente,
                                    Data = a.Data
                                });

            return View(atendimentos);
        }

        public ActionResult Details(int id)
        {
            var atendimento = db.Atendimentos.Find(id);
            var consultor = db.AspNetUsers.Find(atendimento.Id_Consultor);
            var visitante = db.Visitante.Find(atendimento.Id_Visitante);
            var mensagens = db.Mensagens.Where(x => x.Id_Atendimento == id).ToList();

            var model = new AtendimentoDetalhesViewModel
            {
                Atendimento = atendimento,
                Consultor = consultor,
                Visitante = visitante,
                Mensagens = mensagens,
            };

            return View(model);
        }
    }
}