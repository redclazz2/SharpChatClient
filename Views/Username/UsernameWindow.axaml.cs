using Avalonia.Controls;
using Avalonia.Interactivity;

namespace SharpClient;

public partial class UsernameWindow : Window
{
    public UsernameWindow()
    {
        InitializeComponent();
    }

    private void OnOkClick(object sender, RoutedEventArgs e)
    {
        var username = UsernameInput.Text;

        if (!string.IsNullOrWhiteSpace(username))
        {
            Close(username);
        }
    }
}

