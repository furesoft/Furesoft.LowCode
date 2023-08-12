using Avalonia.Controls;
using Avalonia.PropertyGrid.Controls;
using Avalonia.PropertyGrid.Controls.Factories;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using TextMateSharp.Grammars;

namespace Furesoft.LowCode;

internal class EvaluatableCellEditFactory : AbstractCellEditFactory
{
    private readonly RegistryOptions options = new(ThemeName.Dark);
    private TextMate.Installation _textmate;
    public override int ImportPriority => base.ImportPriority - 100000;

    public override Control HandleNewProperty(PropertyCellContext context)
    {
        if (context.Property.PropertyType != typeof(Evaluatable))
        {
            return null;
        }

        var control = new TextEditor();
        control.TextChanged += (s, e) =>
        {
            SetAndRaise(context, control, new Evaluatable(control.Text));
        };

        _textmate = control.InstallTextMate(options);

        _textmate.SetGrammar(options.GetScopeByExtension(".js"));

        return control;
    }

    public override bool HandlePropertyChanged(PropertyCellContext context)
    {
        if (context.Property.PropertyType != typeof(Evaluatable))
        {
            return false;
        }

        ValidateProperty(context.CellEdit, context.Property, context.Target);

        if (context.CellEdit is TextEditor ts)
        {
            var value = (Evaluatable)context.Property.GetValue(context.Target);
            ts.Document.Text = value?.Source;
            
            ts.InvalidateVisual();

            return true;
        }

        return false;
    }
}
