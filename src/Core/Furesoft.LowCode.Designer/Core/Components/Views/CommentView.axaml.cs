using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Furesoft.LowCode.Designer.Core.Components.Views;

public partial class CommentView : UserControl
{
    public CommentView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

