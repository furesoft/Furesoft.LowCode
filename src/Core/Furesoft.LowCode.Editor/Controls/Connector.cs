using Avalonia.Controls.Metadata;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace Furesoft.LowCode.Editor.Controls;

[PseudoClasses(":selected")]
public class Connector : Shape
{
    public static readonly StyledProperty<Point> StartPointProperty =
        AvaloniaProperty.Register<Connector, Point>(nameof(StartPoint));

    public static readonly StyledProperty<Point> EndPointProperty =
        AvaloniaProperty.Register<Connector, Point>(nameof(EndPoint));

    public static readonly StyledProperty<double> OffsetProperty =
        AvaloniaProperty.Register<Connector, double>(nameof(Offset));

    static Connector()
    {
        StrokeThicknessProperty.OverrideDefaultValue<Connector>(1);
        AffectsGeometry<Connector>(StartPointProperty, EndPointProperty, OffsetProperty);
    }

    public Point StartPoint
    {
        get => GetValue(StartPointProperty);
        set => SetValue(StartPointProperty, value);
    }

    public Point EndPoint
    {
        get => GetValue(EndPointProperty);
        set => SetValue(EndPointProperty, value);
    }

    public double Offset
    {
        get => GetValue(OffsetProperty);
        set => SetValue(OffsetProperty, value);
    }

    protected override Geometry CreateDefiningGeometry()
    {
        var geometry = new StreamGeometry();

        using var context = geometry.Open();

        context.BeginFigure(StartPoint, false);

        if (DataContext is IConnector connector)
        {
            var p1X = StartPoint.X;
            var p1Y = StartPoint.Y;
            var p2X = EndPoint.X;
            var p2Y = EndPoint.Y;

            connector.GetControlPoints(
                connector.Orientation,
                Offset,
                connector.Start?.Alignment ?? PinAlignment.Left,
                connector.End?.Alignment ?? PinAlignment.Left,
                ref p1X, ref p1Y,
                ref p2X, ref p2Y);

            if (connector.Start.Parent == connector.End.Parent)
            {
                if (connector.Start.Alignment == PinAlignment.Top && connector.End.Alignment == PinAlignment.Bottom
                    || connector.Start.Alignment == PinAlignment.Bottom && connector.End.Alignment == PinAlignment.Top)
                {
                    context.LineTo(new(p1X, p1Y));
                    context.LineTo(new(p1X + connector.Start.Parent.Width, p1Y));
                    context.LineTo(new(p2X + connector.Start.Parent.Width, p2Y));
                    context.LineTo(new(p2X, p2Y));
                    context.LineTo(EndPoint);
                }
            }
            else
            {
                var midY = SnapHelper.Snap((p1Y + p2Y) / 2, connector.Parent.SnapY);

                context.LineTo(new(p1X, p1Y));
                context.LineTo(new(EndPoint.X, midY));
                context.LineTo(EndPoint);
            }
        }
        else
        {
            context.LineTo(StartPoint);
        }

        context.EndFigure(false);

        return geometry;
    }
}
