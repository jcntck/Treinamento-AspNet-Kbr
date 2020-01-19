using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreinamentoAspNet02.Models
{
    public class Consultores
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Descricao { get; set; }
        public string FotoPerfil { get; set; }
        public bool Ocupado { get; set; }
    }
}