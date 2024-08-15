using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using SharpClient.ChatApplication;
using SharpClient.Domain.Entity;
using SharpClient.Domain.Port;
using SharpClient.Infrastructure.Controller;
using SharpClient.Infrastructure.Model;
using SharpClient.Infrastructure.Router;

namespace SharpClient;

public partial class MainWindow : Window
{
    public IChatRouter router;
    public ChatController controller;
    private string? inputUsername;

    public MainWindow()
    {
        InitializeComponent();
        controller = new ChatController(
            new ChatData()
        );

        router = new ChatRouter(
            controller
        );

        DataContext = controller.chatData;
        Opened += OnOpened;
        Closing += OnClose;
    }

    private async void OnOpened(object? sender, EventArgs e)
    {
        if (!router.Init())
        {
            var errorWindow = new ErrorWindow();
            await errorWindow.ShowDialog(this);
            Close();
            return;
        }
        else
        {
            var usernameWindow = new UsernameWindow();
            inputUsername = await usernameWindow.ShowDialog<string>(this);

            router.Write(
                new Transaction("Connect", inputUsername)
            );
        }
    }

    private void OnClose(object? sender, EventArgs e)
    {
        router.Write(new Transaction("Disconnect",""));
        router.Close();
    }

    private void OnMessageBoxClick(object sender, RoutedEventArgs e)
    {
        var messageBody = MessageInput.Text;

        ChatMessage message = new ChatMessage()
        {
            username = inputUsername!,
            body = messageBody!
        };

        router.Write(new Transaction(
            "Message",
            message

        ));

        MessageInput.Text = "";

        controller.chatData.AddMessage(message);
    }

    private void OnServerFailed()
    {
        Dispatcher.UIThread.Post(Close);
    }
}