using Furesoft.LowCode.Editor.MVVM;

namespace Furesoft.LowCode.Editor.Controls;

using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

public class PinShape : Shape
{
    public static readonly StyledProperty<PinAlignment> PinAlignmentProperty =
        AvaloniaProperty.Register<PinShape, PinAlignment>(nameof(PinAlignment));

    public static readonly StyledProperty<PinMode> ModeProperty =
        AvaloniaProperty.Register<PinShape, PinMode>(nameof(Mode), PinMode.Output);

    public PinAlignment PinAlignment
    {
        get => GetValue(PinAlignmentProperty);
        set => SetValue(PinAlignmentProperty, value);
    }

    public PinMode Mode
    {
        get => GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    protected override Geometry CreateDefiningGeometry()
    {
        if (Mode == PinMode.Input)
        {
            return new EllipseGeometry(new(new(Width, Height)));
        }
        else
        {
            var geometry = new StreamGeometry();
            using var context = geometry.Open();

            var width = Width;
            var height = Height;

            switch (PinAlignment)
            {
                case PinAlignment.Left:
                    context.BeginFigure(new(0, height / 2 - 1), true);
                    context.LineTo(new(width, -width));
                    context.LineTo(new(width, height * 1.2));
                    break;
                case PinAlignment.Right:
                    context.BeginFigure(new(width * 1.2, height / 2 - 1), true);
                    context.LineTo(new(0, -width / 2));
                    context.LineTo(new(0, height * 1.2));
                    break;
                case PinAlignment.Top:
                    context.BeginFigure(new(width / 2 - 0.75, 0), true);
                    context.LineTo(new(-width / 2, height * 1.2));
                    context.LineTo(new(width * 1.2, height));
                    break;
                case PinAlignment.Bottom:
                    context.BeginFigure(new(width / 2 - 0.75, height * 1.2), true);
                    context.LineTo(new(-(width / 2), 0));
                    context.LineTo(new(width * 1.2, 0));
                    break;
            }

            context.EndFigure(true);

            return geometry;
        }
    }
}
