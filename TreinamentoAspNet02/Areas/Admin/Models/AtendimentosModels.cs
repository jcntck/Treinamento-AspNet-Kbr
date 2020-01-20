using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TreinamentoAspNet02.Entity;

namespace TreinamentoAspNet02.Areas.Admin.Models
{
	public class AtendimentosViewModel
    {
        public int Id { get; set; }
        public string NomeConsultor { get; set; }
        public string NomeCliente { get; set; }
        public DateTime? Data { get; set; }
    }

    public class AtendimentoDetalhesViewModel
    {
        public Atendimentos Atendimento { get; set; }
        public AspNetUsers Consultor { get; set; }
        public Visitante Visitante { get; set; }
        public List<Mensagens> Mensagens { get; set; }
    }
}