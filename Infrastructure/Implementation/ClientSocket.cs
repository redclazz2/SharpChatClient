using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SharpClient.Domain.Port;
using SharpClient.Infrastructure.Controller;
using SharpSocket.Domain.Interface.Client;
using SharpSocket.Helper;

namespace SharpSocket.infrastructure.Client
{
    public class ClientSocket : IClientSocket
    {
        Socket socket;
        IPEndPoint ipEndPoint;
        IChatRouter router;
        CancellationToken token;

        public ClientSocket(int port, string url, ProtocolType protocolType, IChatRouter router, CancellationToken token)
        {
            this.router = router;
            IPAddress localIpAddress = IPAddress.Parse(url);
            ipEndPoint = new(localIpAddress, port);
            this.token = token;

            socket = new(
                ipEndPoint.AddressFamily,
                SocketType.Stream,
                protocolType
            );
        }

        public bool Close()
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Connect()
        {
            try
            {
                socket.Connect(ipEndPoint);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log(LoggerLevel.Error,"ClientSocket",e.Message);
                return false;
            }
        }

        public void Read()
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var helperBuffer = new byte[4];
                    socket.Receive(helperBuffer);

                    token.ThrowIfCancellationRequested();

                    int messageLength = SharpClient.Helper.BitConverter.ToInt32(helperBuffer, 0);
                    var buffer = new byte[messageLength];
                    var read = 0;

                    while (read < messageLength)
                    {
                        read += socket.Receive(buffer, SocketFlags.None);
                    }

                    router.Route(buffer);
                }
                catch
                {
                    router.Close();
                    Logger.Log(LoggerLevel.Info, "ClientSocket", "Disconnected from server");
                }
            }
            Logger.Log(LoggerLevel.Info, "ClientSocket", "Closing Read Thread");
        }

        public async void Write(byte[] data)
        {
            await socket.SendAsync(SharpClient.Helper.BitConverter.GetBytes(data.Length), SocketFlags.None);
            await socket.SendAsync(data, SocketFlags.None);
        }
    }
}