using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        //private sistema_atendimentoEntities db = new sistema_atendimentoEntities();
        private sistema_atendimentoEntities1 db = new sistema_atendimentoEntities1();

        #endregion

        public Task Send(string name, string message, string group)
        {
            return Clients.Group(group).addNewMessageToPage(name, message);
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
                //var consultor = db.AspNetUsers.FirstOrDefault(x => x.UserName == Context.User.Identity.Name);
                //if (consultor != null)
                //{
                //    consultor.Ocupado = true;
                //    db.SaveChanges();
                //}

                var item = ConnectedUsers.FirstOrDefault(x => x.UserName == Context.User.Identity.Name);
                if (item == null)
                {
                    ConnectedUsers.Add(new UserDetail
                    {
                        ConnectionId = id,
                        UserName = userName,
                    });
                }
                else
                {
                    Clients.Client(item.ConnectionId).errorMessage();
                    item.ConnectionId = id;
                }
                Clients.All.gerarListagem(ConnectedUsers);
            }
            else
            {
                if (ConnectedVisitantes.Count(x => x.IdAtendimento == idAtendimento) == 0)
                {
                    ConnectedVisitantes.Add(new VisitanteDetail { ConnectionId = id, IdAtendimento = (int)idAtendimento, AtendimentoIniciado = false, TempoSobrando = -1 });
                }
                else
                {
                    var visitante = ConnectedVisitantes.FirstOrDefault(x => x.IdAtendimento == idAtendimento);
                    visitante.ConnectionId = id;
                }
            }
        }

        public async Task JoinRoom(string roomName, string nameUser, bool isConsultor)
        {
            var item = GroupsControl.FirstOrDefault(x => x.Name == roomName);
            if (!isConsultor)
            {
                var consultor = db.AspNetUsers.Find(roomName);
                if (consultor != null)
                {
                    consultor.Ocupado = true;
                    db.SaveChanges();

                    var visitante = ConnectedVisitantes.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
                    GroupsControl.Add(new GroupsDetail { Name = consultor.Id, ConnectionId = Context.ConnectionId });
                    Status(consultor.Id, true);

                    await Clients.Group(roomName).novoAtendimento(visitante.IdAtendimento, nameUser);
                }
            }

            await Clients.Client(Context.ConnectionId).aviso();
            await Groups.Add(Context.ConnectionId, roomName);
        }

        public void LeaveRoom(string roomName)
        {
            var consultor = db.AspNetUsers.Find(roomName);
            if (consultor != null)
            {
                consultor.Ocupado = false;
                db.SaveChanges();
                Status(consultor.Id, false);

                Groups.Remove(Context.ConnectionId, roomName);
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

            var visitante = ConnectedVisitantes.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (visitante != null) Desconectar(visitante.IdAtendimento);

            return base.OnDisconnected(stopCalled);
        }

        #region Auxiliares
        public void IniciarListagem()
        {
            Clients.All.gerarListagem(ConnectedUsers);
        }

        public void Desconectar(int idAtendimento)
        {
            var item = ConnectedVisitantes.FirstOrDefault(x => x.IdAtendimento == idAtendimento);
            if (item != null)
            {
                var atendimento = db.Atendimentos.Find(item.IdAtendimento);
                if (atendimento != null)
                {
                    atendimento.Encerrado = true;
                }

                var consultor = db.AspNetUsers.Find(atendimento.Id_Consultor);
                if (consultor != null)
                {
                    consultor.Ocupado = true;
                    var itemConsultor = ConnectedUsers.FirstOrDefault(x => x.UserName == consultor.UserName);
                    if (itemConsultor != null)
                    {
                        Clients.Client(itemConsultor.ConnectionId).atendimentoEncerrado(atendimento.Id);
                    }

                    var group = GroupsControl.FirstOrDefault(x => x.Name == consultor.Id);
                    if (group != null)
                    {
                        LeaveRoom(group.Name);
                        GroupsControl.Remove(group);
                    }
                }

                db.SaveChanges();
                ConnectedVisitantes.Remove(item);
            }

        }

        public void saveTime(int duracao, string roomName, int idAtendimento)
        {
            var atendimento = ConnectedVisitantes.FirstOrDefault(x => x.IdAtendimento == idAtendimento);
            if (atendimento != null)
            {
                if (!atendimento.AtendimentoIniciado)
                {
                    atendimento.AtendimentoIniciado = true;
                    atendimento.TempoSobrando = duracao;
                    Clients.Group(roomName).timer(duracao);
                }

                if (atendimento.TempoSobrando > duracao) atendimento.TempoSobrando = duracao;
                Clients.Group(roomName).timer(atendimento.TempoSobrando);

                if (duracao == 0)
                {
                    atendimento.TempoSobrando = -1;
                    Desconectar(idAtendimento);
                    Clients.Client(Context.ConnectionId).encerrarAtendimento();
                }
            }

        }

        public void keyPress(string nome, string idConsultor, int idAtendimento)
        {
            var consultor = db.AspNetUsers.Find(idConsultor);
            var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                var visitante = ConnectedVisitantes.FirstOrDefault(x => x.IdAtendimento == idAtendimento);
                if (visitante != null)
                {
                    Clients.Client(visitante.ConnectionId).digitando(consultor.Nome);
                }
            } else
            {
                var itemConsultor = ConnectedUsers.FirstOrDefault(x => x.UserName == consultor.UserName);
                if (itemConsultor != null)
                {
                    Clients.Client(itemConsultor.ConnectionId).digitando(nome);
                }
            }
        }

        public void clean(string idConsultor, int idAtendimento)
        {
            var consultor = db.AspNetUsers.Find(idConsultor);
            var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                var visitante = ConnectedVisitantes.FirstOrDefault(x => x.IdAtendimento == idAtendimento);
                if (visitante != null)
                {
                    Clients.Client(visitante.ConnectionId).clean();
                }
            }
            else
            {
                var itemConsultor = ConnectedUsers.FirstOrDefault(x => x.UserName == consultor.UserName);
                if (itemConsultor != null)
                {
                    Clients.Client(itemConsultor.ConnectionId).clean();
                }
            }
        }
        #endregion
    }
}