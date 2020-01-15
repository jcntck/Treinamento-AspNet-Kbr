using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace TreinamentoAspNet02.Chat
{
    public class ChatHub : Hub
    {
        // Tutorial da Microsoft
        public void Send(string name, string message, string group)
        {
            // Call the addNewMessageToPage method to update clients.
            message = DateTime.Now.ToString("HH:mm:ss") + " - " + message;
            Clients.Group(group).addNewMessageToPage(name, message);
        }

        public void AdicionaGrupo(string grupo)
        {
            //(caller)Manda uma chamada de método somente para quem requisitou o
            //adicionarGrupo
            Clients.Group(grupo).Send("Aviso", $"{Context.ConnectionId} has joined the group {grupo}.", grupo);
            Groups.Add(Context.ConnectionId, grupo);
        }

        public void RemoveGrupo(string grupo)
        {
            Clients.Caller.send("Aviso", "Saiu do grupo:" + grupo, grupo);
            Groups.Remove(Context.ConnectionId, grupo);
        }
    }

}