using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TreinamentoAspNet02.Models
{
    public class CreateViewModel
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Required]
        [Phone]
        [Display(Name = "Celular")]
        public string Celular { get; set; }
        public string IdConsultor { get; set; }
    }
}