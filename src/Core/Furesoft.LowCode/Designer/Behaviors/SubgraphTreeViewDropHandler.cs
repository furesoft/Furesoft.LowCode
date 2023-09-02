using Avalonia.Input;
using Avalonia.Xaml.Interactions.DragAndDrop;
using Furesoft.LowCode.ProjectSystem.Items;

namespace Furesoft.LowCode.Designer.Behaviors;

public class SubgraphTreeViewDropHandler : DropHandlerBase
{
    public override bool Validate(object sender, DragEventArgs e, object sourceContext, object targetContext,
        object state)
    {
        if (sender is TreeView tv)
        {
            tv.SelectedItem = null;
        }

        return sourceContext is GraphItem;
    }
}
