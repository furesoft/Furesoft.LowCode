namespace Furesoft.LowCode.Editor.Model;

public static class ConnectorExtensions
{
    public static void GetControlPoints(
        this IConnector connector,
        ConnectorOrientation orientation,
        double offset,
        PinAlignment p1A,
        PinAlignment p2A,
        ref double p1X,
        ref double p1Y,
        ref double p2X,
        ref double p2Y)
    {
        switch (orientation)
        {
            case ConnectorOrientation.Auto:
                AutoOrientation(offset, p1A, p2A, ref p1X, ref p1Y, ref p2X, ref p2Y);

                break;
            case ConnectorOrientation.Horizontal:
                p1X += offset;
                p2X -= offset;
                break;
            case ConnectorOrientation.Vertical:
                p1Y += offset;
                p2Y -= offset;
                break;
        }

        p1X = SnapHelper.Snap(p1X, connector.Parent.SnapX);
        p1Y = SnapHelper.Snap(p1Y, connector.Parent.SnapY);

        p2X = SnapHelper.Snap(p2X, connector.Parent.SnapX);
        p2Y = SnapHelper.Snap(p2Y, connector.Parent.SnapY);
    }

    private static void AutoOrientation(double offset, PinAlignment p1A, PinAlignment p2A, ref double p1X,
        ref double p1Y,
        ref double p2X, ref double p2Y)
    {
        switch (p1A)
        {
            case PinAlignment.Left:
                AutoLeft(offset, p2A, ref p1X, ref p2X, ref p2Y);
                break;
            case PinAlignment.Right:
                AutoRight(offset, p2A, ref p1X, ref p2X, ref p2Y);
                break;
            case PinAlignment.Top:
                AutoTop(offset, p2A, ref p1Y, ref p2X, ref p2Y);
                break;
            case PinAlignment.Bottom:
                AutoBottom(offset, p2A, ref p1Y, ref p2Y);
                break;
        }
    }

    private static void AutoLeft(double offset, PinAlignment p2A, ref double p1X, ref double p2X, ref double p2Y)
    {
        switch (p2A)
        {
            case PinAlignment.Left:
                p1X -= offset;
                p2X -= offset;
                break;
            case PinAlignment.Right:
                p1X -= offset;
                p2X += offset;
                break;
            case PinAlignment.Top:
                p1X -= offset;
                break;
            case PinAlignment.Bottom:
                p2Y += offset;
                break;
        }
    }

    private static void AutoRight(double offset, PinAlignment p2A, ref double p1X, ref double p2X, ref double p2Y)
    {
        switch (p2A)
        {
            case PinAlignment.Left:
                p1X += offset;
                p2X -= offset;
                break;
            case PinAlignment.Right:
                p1X += offset;
                p2X += offset;
                break;
            case PinAlignment.Top:
                p1X += offset;
                break;
            case PinAlignment.Bottom:
                p2Y += offset;
                break;
        }
    }

    private static void AutoTop(double offset, PinAlignment p2A, ref double p1Y, ref double p2X, ref double p2Y)
    {
        switch (p2A)
        {
            case PinAlignment.Left:
                p2X -= offset;
                break;
            case PinAlignment.Right:
                p2X += offset;
                break;
            case PinAlignment.Top:
                p1Y -= offset;
                p2Y -= offset;
                break;
            case PinAlignment.Bottom:
                p1Y -= offset;
                p2Y += offset;
                break;
        }
    }

    private static void AutoBottom(double offset, PinAlignment p2A, ref double p1Y, ref double p2Y)
    {
        switch (p2A)
        {
            case PinAlignment.Left:
                p1Y += offset;
                break;
            case PinAlignment.Right:
                p1Y += offset;
                break;
            case PinAlignment.Top:
                p1Y += offset;
                p2Y -= offset;
                break;
            case PinAlignment.Bottom:
                p1Y += offset;
                p2Y += offset;
                break;
        }
    }
}
