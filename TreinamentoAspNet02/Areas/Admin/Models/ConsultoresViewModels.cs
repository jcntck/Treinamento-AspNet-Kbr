using System;
using System.Collections.Generic;
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
}