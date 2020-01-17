using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            Clients.Group(group).addNewMessageToPage(name, message);
        }

        public void Notification(string name, string message, string group)
        {
            Clients.Group(group).addNewNotificationToPage(name);
        }

        public async Task JoinRoom(string roomName, string nameUser)
        {
            await Groups.Add(Context.ConnectionId, roomName);
            await Clients.Group(roomName).addNewNotificationToPage(nameUser + " entrou na sala.");
        }

        public async Task LeaveRoom(string roomName, string nameUser)
        {
            await Groups.Remove(Context.ConnectionId, roomName);
            await Clients.Group(roomName).addNewNotificationToPage(nameUser + " deixou a sala");
        }
    }

}