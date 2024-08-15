using System.Net.Sockets;
using System.Threading;
using SharpClient.Domain.Port;
using SharpClient.Infrastructure.Controller;
using SharpSocket.Domain.Interface.Client;
using SharpSocket.Helper;

namespace SharpSocket.infrastructure.Client
{
    public class ClientProcess : IClientProcess
    {
        private readonly string name = "ClientProcess";
        private readonly int port;
        private readonly ProtocolType protocolType;
        private ClientSocket? socket;
        private readonly IChatRouter router;
        public bool connected = false;
        readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public ClientProcess(int port, ProtocolType protocolType, IChatRouter router)
        {
            this.port = port;
            this.protocolType = protocolType;
            this.router = router;
        }

        public bool Init()
        {
            socket = new ClientSocket(
                port,
                "127.0.0.1",
                protocolType,
                router,
                cancellationTokenSource.Token
            );

            return Connect();
        }

        public void Close()
        {
            cancellationTokenSource.Cancel();
            
            if (socket!.Close())
            {
                connected = false;
                Logger.Log(LoggerLevel.Info, name, "Closed client socket");
            }
            else if(connected)
            {
                Logger.Log(LoggerLevel.Warning, name, "Failed to safely close socket");
            }
        }

        private bool Connect()
        {
            if (!connected)
            {
                if (socket?.Connect() ?? false)
                {
                    connected = true;
                    Logger.Log(LoggerLevel.Info, name, "Connected to server");

                    Thread newThread = new Thread(socket.Read);
                    newThread.Start();

                    Logger.Log(LoggerLevel.Info, name, "Read thread started");

                    return true;
                }
                else
                {
                    Logger.Log(LoggerLevel.Error, name, "Failed connection to server");
                    return false;
                }
            }
            else
            {
                Logger.Log(LoggerLevel.Error, name, "Socket already connected");
                return false;
            }
        }

        public void Write(byte[] data)
        {
            if (connected && socket != null)
            {
                socket.Write(data);
            }
            else
            {
                Logger.Log(LoggerLevel.Error, name, "Socket not connected.");
            }
        }
    }
}