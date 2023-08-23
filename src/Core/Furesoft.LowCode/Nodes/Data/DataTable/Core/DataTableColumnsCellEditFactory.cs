using Avalonia.PropertyGrid.Controls;
using Avalonia.PropertyGrid.Controls.Factories;

namespace Furesoft.LowCode.Nodes.Data.DataTable.Core;

public class DataTableColumnsCellEditFactory : AbstractCellEditFactory
{
    public override Control HandleNewProperty(PropertyCellContext context)
    {
        if (context.Property.PropertyType != typeof(ObservableCollection<ColumnDataDefinition>))
        {
            return null;
        }


        var btn = new Button();
        btn.Content = "...";
        btn.Tag = context.Property.GetValue(context.Target);
        btn.Click += (sender, _) =>
        {
            var wndw = new ColumnsWindow();
            wndw.DataContext = context.Target;

            wndw.Show();
        };

        return btn;
    }

    public override bool HandlePropertyChanged(PropertyCellContext context)
    {
        if (context.Property.PropertyType != typeof(ObservableCollection<ColumnDataDefinition>))
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
