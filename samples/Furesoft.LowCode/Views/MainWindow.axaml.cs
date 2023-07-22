using Avalonia;
using Avalonia.Controls;

namespace Furesoft.LowCode.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }
}
