using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TreinamentoAspNet02.Areas.Consultor.Models;
using TreinamentoAspNet02.Entity;

namespace TreinamentoAspNet02.Areas.Consultor.Controllers
{
    [Authorize(Roles = "Consultor")]
    public class HomeController : Controller
    {
        //private sistema_atendimentoEntities db = new sistema_atendimentoEntities();
        private sistema_atendimentoEntities1 db = new sistema_atendimentoEntities1();

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
            var dictionary = new Dictionary<string, object>();

            var user = db.AspNetUsers.First(x => x.UserName == User.Identity.Name);
            dictionary.Add("ConsultorOcupado", user.Ocupado);
            if (user.Ocupado)
            {
                var atendimento = db.Atendimentos.OrderByDescending(a => a.Id).FirstOrDefault(x => x.Id_Consultor == user.Id);
                dictionary.Add("IdAtendimento", atendimento.Id);
                dictionary.Add("AtendimentoEncerrado", atendimento.Encerrado);
            }

            return Json(dictionary);
        }

        public JsonResult GetMensagens(int idAtendimento)
        {
            List<MensagemModel> mensagens = new List<MensagemModel>();
            var entity = db.Mensagens.Where(x => x.Id_Atendimento == idAtendimento).ToList();
            foreach (var mensagem in entity)
            {
                mensagens.Add(new MensagemModel
                {
                    Id = mensagem.Id,
                    Mensagem = mensagem.Mensagem,
                    // Arquivo
                    idConsultor = mensagem.enviadoPorConsultor,
                    idVisitante = mensagem.enviadoPorVisitante,
                    idAtendimento = mensagem.Id_Atendimento
                });
            }
            return Json(mensagens);
        }
    }
}