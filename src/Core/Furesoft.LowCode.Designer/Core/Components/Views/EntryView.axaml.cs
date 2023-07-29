using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Furesoft.LowCode.Designer.Core.Components.Views;

public partial class EntryView : UserControl
{
    public EntryView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

