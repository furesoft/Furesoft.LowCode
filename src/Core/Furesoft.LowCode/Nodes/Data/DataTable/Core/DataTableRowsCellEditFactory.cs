using Avalonia.PropertyGrid.Controls;
using Avalonia.PropertyGrid.Controls.Factories;

namespace Furesoft.LowCode.Nodes.Data.DataTable.Core;

public class DataTableRowsCellEditFactory : AbstractCellEditFactory
{
    public override Control HandleNewProperty(PropertyCellContext context)
    {
        if (context.Property.PropertyType != typeof(ObservableCollection<RowDataDefinition>))
        {
            return null;
        }


        var btn = new Button();
        btn.Content = "...";
        btn.Tag = context.Property.GetValue(context.Target);
        btn.Click += (sender, _) =>
        {
            var wndw = new RowsWindow();
            wndw.DataContext = context.Target;

            wndw.Show();
        };

        return btn;
    }

    public override bool HandlePropertyChanged(PropertyCellContext context)
    {
        if (context.Property.PropertyType != typeof(ObservableCollection<RowDataDefinition>))
        {
            return false;
        }

        ValidateProperty(context.CellEdit, context.Property, context.Target);

        if (context.CellEdit is Button)
        {
            return true;
        }

        return true;
    }
}
