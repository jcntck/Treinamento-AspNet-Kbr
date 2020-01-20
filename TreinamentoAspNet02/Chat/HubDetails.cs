using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreinamentoAspNet02.Chat
{
	public class UserDetail
    {
        public string UserName { get; set; }
        public string ConnectionId { get; set; }
    }

    public class VisitanteDetail
    {
        public string ConnectionId { get; set; }
        public int IdAtendimento { get; set; }
    }

    public class GroupsDetail
    {
        public string Name { get; set; }
        public string ConnectionId { get; set; }
    }
}