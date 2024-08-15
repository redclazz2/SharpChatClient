using SharpClient.Domain.Entity;

namespace SharpClient.Domain.Port
{
    public interface IChatController
    {
        public void HandleWelcome(string[] usernames);
        public void HandleConnection(string username);
        public void HandleDisconnection(string username);
        public void HandleMessage(ChatMessage message); 
    }
}