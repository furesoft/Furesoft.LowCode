using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NodeEditorDemo.Core.Components.Views;

public partial class TextNodeView : UserControl
{
    public TextNodeView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

