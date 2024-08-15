using System;
using SharpClient.ChatApplication;
using SharpClient.Domain.Entity;
using SharpClient.Domain.Port;
using Tmds.DBus.Protocol;

namespace SharpClient.Infrastructure.Controller
{
    public class ChatController : IChatController
    {
        public ChatData chatData;

        public ChatController(ChatData chatData)
        {
            this.chatData = chatData;
        }

        public void HandleWelcome(string[] usernames)
        {
            chatData.Identified = true;
            chatData.AddUsernames(usernames);
            chatData.AddMessage(new ChatMessage()
            {
                username = "[SYSTEM]",
                body = "Welcome to the chatroom"
            });
        }

        public void HandleConnection(string username)
        {
            if (chatData.Identified)
            {
                chatData.AddMessage(new ChatMessage()
                {
                    username = "[SYSTEM]",
                    body = $"User: {username} joined the chatroom"
                });
                chatData.AddUsername(username);
            }
        }

        public void HandleDisconnection(string username)
        {
            chatData.RemoveUsername(username);
        }

        public void HandleMessage(ChatMessage message)
        {
            chatData.AddMessage(message);
        }
    }
}