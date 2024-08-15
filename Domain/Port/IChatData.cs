using SharpClient.Domain.Entity;

namespace SharpClient.Domain.Port
{
    public interface IChatData
    {
        public void AddUsername(string username);
        public void AddUsernames(string[] usernames);
        public void RemoveUsername(string username);
        public void AddMessage(ChatMessage message);
    }
}