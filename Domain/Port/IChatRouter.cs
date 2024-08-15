using SharpClient.Infrastructure.Model;

namespace SharpClient.Domain.Port
{
    public interface IChatRouter
    {
        public bool Init();
        public void Close();
        public void Write(Transaction data);
        public void Route(byte[] data);
        public void RouteWelcome(object data);
        public void RouteConnect(object data);
        public void RouteDisconnect(object data);
        public void RouteMessage(object data);
    }
}