using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
//using TreinamentoAspNet02.Entity;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace TreinamentoAspNet02.Chat
{
    public class ChatHub : Hub
    {
        #region Data Members
        static List<UserDetail> ConnectedUsers = new List<UserDetail>();
        #endregion

        //private sistema_atendimentoEntities db = new sistema_atendimentoEntities();
        // Tutorial da Microsoft
        public void Send(string name, string message, string group)
        { 
            Clients.Group(group).addNewMessageToPage(name, message);
        }

        public void Notification(string name, string message, string group)
        {
            Clients.Group(group).addNewNotificationToPage(name);
        }

        public void Status(string idConsultor, bool status)
        {
            Clients.All.statusConsultor(idConsultor, status);
        }

        public void Connect(string userName)
        {
            var id = Context.ConnectionId;

            if (ConnectedUsers.Count(x => x.ConnectionId == id) == 0)
            {
                ConnectedUsers.Add(new UserDetail { ConnectionId = id, UserName = userName });

                Clients.All.gerarListagem(ConnectedUsers);
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                ConnectedUsers.Remove(item);

                var id = Context.ConnectionId;
                Clients.All.gerarListagem(ConnectedUsers);
            }

            return base.OnDisconnected(stopCalled);
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

        #region Auxiliares
        public void IniciarListagem()
        {
            Clients.All.gerarListagem(ConnectedUsers);
        }
        #endregion
    }
}