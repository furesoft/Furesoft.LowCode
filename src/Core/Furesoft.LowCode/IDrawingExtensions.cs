using Furesoft.LowCode.Designer;
using Furesoft.LowCode.Editor.Model;

namespace Furesoft.LowCode;

public static class IDrawingExtensions
{
    public static IEnumerable<CustomNodeViewModel> GetNodes<T>(this IDrawingNode drawing)
    {
        return drawing.Nodes
            .OfType<CustomNodeViewModel>()
            .Where(node => node.DefiningNode.GetType() == typeof(T));
    }
}
