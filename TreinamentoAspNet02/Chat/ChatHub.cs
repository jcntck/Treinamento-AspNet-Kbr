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
        public void Send(string name, string message, string connId)
        { 
            Clients.Client(connId).addNewMessageToPage(name, message);
        }
    }

}