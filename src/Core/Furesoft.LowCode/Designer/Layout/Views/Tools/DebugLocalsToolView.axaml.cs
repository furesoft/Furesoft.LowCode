using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Furesoft.LowCode.Designer.Layout.Views.Tools;

public partial class DebugLocalsToolView : UserControl
{
    public DebugLocalsToolView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

