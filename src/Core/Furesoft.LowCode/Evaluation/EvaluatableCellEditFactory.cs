using Avalonia.PropertyGrid.Controls;
using Avalonia.PropertyGrid.Controls.Factories;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using TextMateSharp.Grammars;

namespace Furesoft.LowCode.Evaluation;

internal class EvaluatableCellEditFactory : AbstractCellEditFactory
{
    private readonly RegistryOptions _options = new(ThemeName.Dark);
    private TextMate.Installation _textmate;
    public override int ImportPriority => base.ImportPriority - 100000;

    public override Control HandleNewProperty(PropertyCellContext context)
    {
        if (context.Property.PropertyType.Name != typeof(Evaluatable<>).Name)
        {
            return null;
        }

        var control = new TextEditor();
        control.TextChanged += (s, e) =>
        {
            var instance = (dynamic)Activator.CreateInstance(context.Property.PropertyType, control.Text);
            try
            {

                instance.Parent = (EmptyNode)context.Target;
            }
            catch (Exception ex)
            {
                
            }
            
            SetAndRaise(context, control, instance);
        };
        control.Text = ((dynamic)context.Property.GetValue(context.Target)).Source;

        _textmate = control.InstallTextMate(_options);

        _textmate.SetGrammar(_options.GetScopeByExtension(".js"));

        return control;
    }

    public override bool HandlePropertyChanged(PropertyCellContext context)
    {
        if (context.Property.PropertyType != typeof(Evaluatable<>))
        {
            return false;
        }

        ValidateProperty(context.CellEdit, context.Property, context.Target);

        if (context.CellEdit is TextEditor ts)
        {
            var value = (dynamic)context.Property.GetValue(context.Target);
            ts.Document.Text = value?.Source;

            ts.InvalidateVisual();

            return true;
        }

        return false;
    }
}
