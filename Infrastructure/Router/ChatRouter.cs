using SharpClient.Domain.Entity;
using SharpClient.Domain.Port;
using SharpClient.Infrastructure.Model;
using SharpSocket.Domain.Interface.Client;
using SharpSocket.Helper;
using SharpSocket.infrastructure.Client;

namespace SharpClient.Infrastructure.Router
{
    public class ChatRouter : IChatRouter
    {
        IChatController controller;
        IClientProcess clientProcess;

        public ChatRouter(IChatController controller)
        {
            this.controller = controller;
            clientProcess = new ClientProcess(
                8059,
                System.Net.Sockets.ProtocolType.Tcp,
                this
            );
        }

        public bool Init()
        {
            return clientProcess.Init();
        }

        public void Close(){
            clientProcess.Close();
        }

        public void Route(byte[] data)
        {
            Logger.Log(LoggerLevel.Info, "ChatController", "Routing a request");
            Transaction transaction = Formatter.Deserialize<Transaction>(data, data.Length);
            switch (transaction.route)
            {
                case "Welcome":
                    Logger.Log(LoggerLevel.Info, "ChatController", "Server welcomes you!");
                    RouteWelcome(transaction.data);
                    break;

                case "Connect":
                    Logger.Log(LoggerLevel.Info, "ChatController", "A new user has connected");
                    RouteConnect(transaction.data);
                    break;

                case "Disconnect":
                    Logger.Log(LoggerLevel.Info, "ChatController", "A user has disconnected");
                    RouteDisconnect(transaction.data);
                    break;

                case "Message":
                    Logger.Log(LoggerLevel.Info, "ChatController", "Message recieved");
                    RouteMessage(transaction.data);
                    break;
            }
        }

        public void RouteConnect(object data)
        {
            string username = data.ToString()!;
            controller.HandleConnection(username);
        }

        public void RouteDisconnect(object data)
        {
            string dcdUsername = data.ToString()!;
            controller.HandleDisconnection(dcdUsername);
        }

        public void RouteMessage(object data)
        {
            ChatMessage message = Formatter.Deserialize<ChatMessage>(data.ToString()!);
            controller.HandleMessage(message);
        }

        public void RouteWelcome(object data)
        {
            string[] usernames = Formatter.Deserialize<string[]>(data.ToString()!);
            controller.HandleWelcome(usernames);
        }

        public void Write(Transaction data)
        {
            clientProcess.Write(Formatter.Serialize(data));
        }
    }
}