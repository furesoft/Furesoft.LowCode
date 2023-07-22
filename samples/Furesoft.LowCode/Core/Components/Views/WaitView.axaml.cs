using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Furesoft.LowCode.Core.Components.Views;

public partial class WaitView : UserControl
{
    public WaitView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

