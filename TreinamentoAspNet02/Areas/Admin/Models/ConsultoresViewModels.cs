using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TreinamentoAspNet02.Areas.Admin.Models
{
    public class IndexViewModel
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Descricao { get; set; }
        public string FotoPerfil { get; set; }
        public string Role { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Nome do consultor")]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "O/A {0} deve ter no mínimo {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [Compare("Password", ErrorMessage = "A senha e a senha de confirmação não correspondem.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Foto de Perfil")]
        public HttpPostedFileBase FotoPerfil { get; set; }

    }

    public class EditViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Nome do consultor")]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }


        public string FotoAntiga { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Foto de Perfil")]
        public HttpPostedFileBase FotoPerfil { get; set; }
    }

    public class ChangePasswordViewModel
    {
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha atual")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova senha")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nova senha")]
        [Compare("NewPassword", ErrorMessage = "A nova senha e a senha de confirmação não correspondem.")]
        public string ConfirmPassword { get; set; }
    }
}