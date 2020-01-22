using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreinamentoAspNet02.Areas.Consultor.Models
{
    public class MensagemModel
    {
        public int Id { get; set; }
        public string Mensagem { get; set; }
        //Arquivo
        public string idConsultor { get; set; }
        public int? idVisitante { get; set; }
        public int? idAtendimento { get; set; }
    }
}