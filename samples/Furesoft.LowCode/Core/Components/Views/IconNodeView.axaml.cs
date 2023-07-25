using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Furesoft.LowCode.Core.Components.Views;

public partial class IconNodeView : UserControl
{
    public IconNodeView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

