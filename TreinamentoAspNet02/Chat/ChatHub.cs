using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using TreinamentoAspNet02.Entity;

namespace TreinamentoAspNet02.Chat
{
    public class ChatHub : Hub
    {
        #region Data Members
        static List<UserDetail> ConnectedUsers = new List<UserDetail>();
        static List<VisitanteDetail> ConnectedVisitantes = new List<VisitanteDetail>();
        static List<GroupsDetail> GroupsControl = new List<GroupsDetail>();
        private sistema_atendimentoEntities db = new sistema_atendimentoEntities();
        #endregion

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

        public void StoreMessage(string message, string idConsultor, int idVisitante, int idAtendimento)
        {
            db.Mensagens.Add(new Mensagens
            {
                Mensagem = message,
                enviadoPorConsultor = idConsultor,
                enviadoPorVisitante = idVisitante,
                Id_Atendimento = idAtendimento
            });
            db.SaveChanges();
        }

        public void Connect(string userName, bool user, int idAtendimento)
        {
            var id = Context.ConnectionId;
            if (user)
            {
                if (ConnectedUsers.Count(x => x.ConnectionId == id) == 0)
                {
                    ConnectedUsers.Add(new UserDetail { ConnectionId = id, UserName = userName });

                    Clients.All.gerarListagem(ConnectedUsers);
                }
            }
            else
            {
                if (ConnectedVisitantes.Count(x => x.ConnectionId == id) == 0)
                {
                    ConnectedVisitantes.Add(new VisitanteDetail { ConnectionId = id, IdAtendimento = (int)idAtendimento });
                }
            }
        }

        public async Task JoinRoom(string roomName, string nameUser)
        {
            var item = GroupsControl.FirstOrDefault(x => x.Name == roomName);
            if (Context.User.Identity.IsAuthenticated)
            {
                await Groups.Add(Context.ConnectionId, roomName);
                await Clients.Group(roomName).addNewNotificationToPage(nameUser + " entrou na sala.");
            }
            else
            {
                var consultor = db.AspNetUsers.Find(roomName);
                if (consultor != null)
                {
                    consultor.Ocupado = true;
                    db.SaveChanges();

                    var visitante = ConnectedVisitantes.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
                    GroupsControl.Add(new GroupsDetail { Name = consultor.Id, ConnectionId = Context.ConnectionId });
                    Status(consultor.Id, true);

                    await Groups.Add(Context.ConnectionId, roomName);
                    await Clients.Group(roomName).novoAtendimento(visitante.IdAtendimento, nameUser);
                }
            }
        }

        public void LeaveRoom(string roomName, string nameUser)
        {
            var consultor = db.AspNetUsers.Find(roomName);
            if (consultor != null)
            {
                consultor.Ocupado = false;
                db.SaveChanges();
                Status(consultor.Id, false);

                Groups.Remove(Context.ConnectionId, roomName);
                Clients.Group(roomName).addNewNotificationToPage(nameUser + " deixou a sala");
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
                if (item != null)
                {
                    ConnectedUsers.Remove(item);

                    Clients.All.gerarListagem(ConnectedUsers);
                }
            }
            else
            {
                var group = GroupsControl.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
                if (group != null)
                {
                    LeaveRoom(group.Name, "Visitante");
                    GroupsControl.Remove(group);
                }

                var item = ConnectedVisitantes.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
                if (item != null)
                {
                    var atendimento = db.Atendimentos.Find(item.IdAtendimento);
                    if (atendimento != null)
                    {
                        atendimento.Encerrado = true;
                        db.SaveChanges();
                    }

                    ConnectedVisitantes.Remove(item);
                }
            }


            return base.OnDisconnected(stopCalled);
        }

        #region Auxiliares
        public void IniciarListagem()
        {
            Clients.All.gerarListagem(ConnectedUsers);
        }
        #endregion
    }
}