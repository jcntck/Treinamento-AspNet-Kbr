using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreinamentoAspNet02.Models
{
    public class Visitantes
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
    }

    public class TempoAtendimento
    {
        public int Duracao { get; set; }
    }
}