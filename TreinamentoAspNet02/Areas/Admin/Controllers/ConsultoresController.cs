using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TreinamentoAspNet02.Models;
using TreinamentoAspNet02.Areas.Admin.Models;
using System.Reflection;
using TreinamentoAspNet02.Entity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using TreinamentoAspNet02.Helpers;
using Microsoft.AspNet.Identity;

namespace TreinamentoAspNet02.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ConsultoresController : Controller
    {
        private ApplicationDbContext Context;
        private ApplicationUserManager _userManager;
        //private sistema_atendimentoEntities db = new sistema_atendimentoEntities();
        private sistema_atendimentoEntities1 db = new sistema_atendimentoEntities1();


        public ConsultoresController()
        {
        }

        public ConsultoresController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpGet]
        // GET: Admin/Consultores
        public ActionResult Index()
        {
            ViewBag.StatusMessage = TempData["success"];
            Context = new ApplicationDbContext();
            var users = (from user in Context.Users
                         select new
                         {
                             Id = user.Id,
                             Nome = user.Nome,
                             Email = user.Email,
                             Descricao = user.Descricao,
                             FotoPerfil = user.FotoPerfil,
                             AutoId = user.AutoId,
                             RoleNames = (from userRole in user.Roles
                                          join role in Context.Roles on userRole.RoleId
                                          equals role.Id
                                          select role.Name).ToList()
                         }).Where(x => x.RoleNames.Contains("Consultor")).OrderByDescending(s => s.AutoId).ToList().Select(p => new Models.IndexViewModel()
                         {
                             Id = p.Id,
                             Nome = p.Nome,
                             Email = p.Email,
                             Descricao = p.Descricao,
                             FotoPerfil = p.FotoPerfil
                         });

            return View(users);
        }

        //
        // GET: /Account/Register
        [Authorize(Roles = "Admin")]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(Models.RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string foto = FileHelper.Save(model.FotoPerfil, "~/Images/Perfil");

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Nome = model.Nome,
                    Descricao = model.Descricao,
                    FotoPerfil = foto
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.SendEmailAsync(user.Id, "Conta criada", "Sua conta no Helpchat foi criada com sucesso.\nEste são seus dados:\nE-mail: " + user.Email + "\nSenha: " + model.Password);
                    UserManager.AddToRole(user.Id, "Consultor");
                    TempData["success"] = "Consultor criado com sucesso.";
                    return RedirectToAction("Index", "Consultores", new { area = "Admin" });
                }
                AddErrors(result);
            }

            // Se chegamos até aqui e houver alguma falha, exiba novamente o formulário
            return View(model);
        }

        //GET: /Account/Edit
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                ViewBag.StatusMessage = TempData["successPassword"];
                var user = UserManager.FindById(id);

                var model = new EditViewModel
                {
                    Id = user.Id,
                    Nome = user.Nome,
                    Email = user.Email,
                    Descricao = user.Descricao,
                    FotoAntiga = user.FotoPerfil
                };

                return View(model);
            }
            return RedirectToAction("Index");
        }

        // POST: /Account/Edit
        [HttpPost]
        [Authorize(Roles = "Admin")]
        //[AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(model.Id);
                string foto = FileHelper.Update(model.FotoAntiga, model.FotoPerfil);

                if (!user.Email.Equals(model.Email))
                {
                    user.UserName = model.Email;
                    user.Email = model.Email;
                }
                user.Nome = model.Nome;
                user.Descricao = model.Descricao;
                user.FotoPerfil = foto;

                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await UserManager.SendEmailAsync(user.Id, "Conta atualizada", "Os dados da sua conta no Helpchat foram atualizados.\nDados atuais:\nNome: " + user.Nome + "\nE-mail: " + user.Email + "\nDescricão: " + user.Descricao);
                    TempData["success"] = "Consultor atualizado com sucesso.";
                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }

            // Se chegamos até aqui e houver alguma falha, exiba novamente o formulário
            return View(model);
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword(string id)
        {
            return View(new Models.ChangePasswordViewModel { Id = id });
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(Models.ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(model.Id, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = UserManager.FindById(model.Id);
                await UserManager.SendEmailAsync(user.Id, "Senha alterada", "Sua senha no Helpchat foram atualizados.\nDados de acesso:\nE-mail:" + user.Email + "\nNova Senha: " + model.NewPassword);
                TempData["successPassword"] = "Senha redefinida com sucesso.";
                return RedirectToAction("Edit", new { id = model.Id });
            }
            AddErrors(result);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteUser(string id)
        {
            var user = UserManager.FindById(id);

            UserManager.SendEmail(user.Id, "Conta excluida", "Sua conta no Helpchat foi deletada.\nData/hora:" + String.Format("{0: dd/MM/yyyy - HH:mm:ss}", DateTime.Now));

            FileHelper.Delete(user.FotoPerfil);
            var result = UserManager.Delete(user);

            if (result.Succeeded)
            {
                TempData["success"] = "Consultor apagado com sucesso.";
                return RedirectToAction("Index");
            }
            AddErrors(result);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Details(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var user = db.AspNetUsers.Find(id);
                return View(user);
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
            }

            base.Dispose(disposing);
        }

        private void AddErrors(IdentityResult result)
        {
            throw new NotImplementedException();
        }
    }
}