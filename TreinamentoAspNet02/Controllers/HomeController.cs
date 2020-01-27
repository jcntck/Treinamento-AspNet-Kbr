using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TreinamentoAspNet02.Chat;
using TreinamentoAspNet02.Entity;
using TreinamentoAspNet02.Helpers;
using TreinamentoAspNet02.Models;

namespace TreinamentoAspNet02.Controllers
{
    public class HomeController : Controller
    {
        //private sistema_atendimentoEntities db = new sistema_atendimentoEntities();
        private sistema_atendimentoEntities1 db = new sistema_atendimentoEntities1();

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

        public ActionResult Chat(int id)
        {
            var atendimentoAtual = db.Atendimentos.Find(id);
            var consultor = db.AspNetUsers.Find(atendimentoAtual.Id_Consultor);

            if (atendimentoAtual != null && !atendimentoAtual.Encerrado && !consultor.Ocupado)
            {
                //var model = new AtendimentoViewModel
                //{
                //    AtendimentoAtual = atendimentoAtual,
                //    Consultor = consultor,
                //    Visitante = db.Visitante.Find(atendimentoAtual.Id_Visitante)
                //};
                //return View(model);
            }
            return RedirectToAction("Index");
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

        public JsonResult StoreFile(HttpPostedFileBase file, int idAtendimento, string who)
        {
            string nameFile = FileHelper.Save(file, "~/Uploads/Atendimento" + idAtendimento);

            var atendimento = db.Atendimentos.Find(idAtendimento);

            //if (who.Equals("consultor"))
            //{
            //    db.Mensagens.Add(new Mensagens
            //    {
            //        Arquivo = nameFile,
            //        enviadoPorConsultor = atendimento.Id_Consultor,
            //        enviadoPorVisitante = 0,
            //        Id_Atendimento = idAtendimento
            //    });
            //} else
            //{
            //    db.Mensagens.Add(new Mensagens
            //    {
            //        Arquivo = nameFile,
            //        enviadoPorConsultor = null,
            //        enviadoPorVisitante = atendimento.Id_Visitante,
            //        Id_Atendimento = idAtendimento
            //    });
            //}

            //db.SaveChanges();

            return Json(nameFile);
        }

        public void Download(string folder, string filename)
        {
            string filePath = Server.MapPath(folder + filename);
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "text/plain";
                Response.Flush();
                Response.TransmitFile(file.FullName);
                Response.End();
            }
        }
    }
}