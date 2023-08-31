using Avalonia.Input;
using Avalonia.Xaml.Interactivity;

namespace Furesoft.LowCode.Editor.Behaviors;

public class SingleClickExpansionBehavior : Behavior<TreeViewItem>
{
    protected override void OnAttached()
    {
        AssociatedObject.PointerReleased += AssociatedObjectOnPointerReleased;
    }

    private void AssociatedObjectOnPointerReleased(object sender, PointerReleasedEventArgs e)
    {
        if (AssociatedObject?.Parent is TreeView tv && tv.SelectedItem is TreeViewItem item)
        {
            item.IsExpanded = !item.IsExpanded;
        }
    }
}
