using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Furesoft.LowCode.Analyzing;
using Furesoft.LowCode.Designer.ViewModels;

namespace Furesoft.LowCode.Designer.Layout.Views.Tools;

public partial class ErrorsToolView : UserControl
{
    
    
    public ErrorsToolView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void InputElement_OnDoubleTapped(object sender, TappedEventArgs e)
    {
        if (sender is ListBox lb && lb.SelectedItem is Message msg)
        {
            ZoomToTarget((EmptyNode)msg.Targets.First());
        }
    }

    private void ZoomToTarget(EmptyNode node)
    {
        var window = (Window)VisualRoot;
        var vm = (MainViewViewModel)window.DataContext;
        NodeZoomBorder zoomControl = vm.SelectedDocument.NodeZoomBorder;

        //ToDo: figure out how to get the coordinates of a node here
        zoomControl.ZoomTo(0.5, node.);
    }
}

