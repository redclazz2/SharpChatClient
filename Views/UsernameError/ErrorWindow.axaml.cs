using Avalonia.Controls;
using Avalonia.Interactivity;

namespace SharpClient;

public partial class ErrorWindow : Window
{
    public ErrorWindow()
    {
        InitializeComponent();
    }

    private void OnOkClick(object sender, RoutedEventArgs e)
    {
        Close();  
    }
}
