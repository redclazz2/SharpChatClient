using System;
using System.Collections.ObjectModel;
using SharpClient.Domain.Entity;
using SharpClient.Domain.Port;

namespace SharpClient.ChatApplication
{
    public class ChatData : IChatData
    {
        public ObservableCollection<string> Users { get; set; } = [];
        public ObservableCollection<ChatMessage> Messages { get; set; } = [];
        public bool Identified = false;

        public void AddMessage(ChatMessage message)
        {
            Messages.Add(message);
        }

        public void AddUsername(string username)
        {
            Users.Add(username);
        }

        public void AddUsernames(string[] usernames)
        {
            foreach (string u in usernames)
            {
                Users.Add(u);
            }
        }

        public void RemoveUsername(string username)
        {
            Users.Remove(username);
        }
    }
}