using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TreinamentoAspNet02.Entity;

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

    public class AtendimentoViewModel
    {
        public Atendimentos AtendimentoAtual { get; set; }
        public AspNetUsers Consultor { get; set; }
        public Entity.Visitante Visitante { get; set; }
    }
}