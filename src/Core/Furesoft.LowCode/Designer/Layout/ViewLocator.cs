#nullable enable
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Dock.Model.Core;
using Furesoft.LowCode.Designer.Layout.ViewModels.Documents;

namespace Furesoft.LowCode.Designer.Layout;

public class ViewLocator : IDataTemplate
{
    public Control Build(object? data)
    {
        var name = data?.GetType().FullName?.Replace("ViewModel", "View");
        if (name is null)
        {
            return new TextBlock {Text = "Invalid Data Type"};
        }

        var type = Type.GetType(name);
        
        if (type is not null)
        {
            var instance = Activator.CreateInstance(type);
            
            if (instance is Control c)
            {
                if (data is DocumentViewModel dvm)
                {
                    var editor = c.FindControl<Editor.Controls.Editor>("EditorControl");
                    editor.PropertyChanged += (sender, args) =>
                    {
                        if (args.Property == Editor.Controls.Editor.ZoomControlProperty)
                        {
                            dvm.NodeZoomBorder = (NodeZoomBorder)args.NewValue;
                        }
                    };
                }

                return c;
            }

            return new TextBlock {Text = "Create Instance Failed: " + type.FullName};
        }

        return new TextBlock {Text = "Not Found: " + name};
    }

    public bool Match(object? data)
    {
        return data is ObservableObject or IDockable;
    }
}
