using Avalonia;
using Avalonia.Controls;
using Furesoft.LowCode.Editor.Controls;

namespace Furesoft.LowCode.Designer.Views;

public partial class MainView : UserControl
{
    public static readonly StyledProperty<NodeZoomBorder> ZoomControlProperty =
        AvaloniaProperty.Register<MenuView, NodeZoomBorder>(nameof(MainView));

    public MainView()
    {
        InitializeComponent();
    }

    public NodeZoomBorder ZoomControl
    {
        get => GetValue(ZoomControlProperty);
        set => SetValue(ZoomControlProperty, value);
    }
}
