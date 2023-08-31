using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using Furesoft.LowCode.Designer.ViewModels;

namespace Furesoft.LowCode.Designer.Behaviors;

public class OpenDocumentOnDoubleTappedBehavior : Behavior<Control>
{
    public static readonly StyledProperty<MainViewViewModel> ViewModelProperty =
        AvaloniaProperty.Register<OpenDocumentOnDoubleTappedBehavior, MainViewViewModel>(nameof(ViewModel));

    public MainViewViewModel ViewModel
    {
        get => GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject is not null)
        {
            AssociatedObject.DoubleTapped += DoubleTapped;
        }
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        if (AssociatedObject is not null)
        {
            AssociatedObject.DoubleTapped -= DoubleTapped;
        }
    }

    private void DoubleTapped(object sender, RoutedEventArgs args)
    {
        if (AssociatedObject is TreeViewItem {Parent: TreeView {SelectedItem: ProjectItem pi}})
        {
            ViewModel.OpenDocument(pi);
        }
    }
}
