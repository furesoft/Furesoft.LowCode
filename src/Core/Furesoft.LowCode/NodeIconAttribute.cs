using Furesoft.LowCode.NodeViews;

namespace Furesoft.LowCode;

[AttributeUsage(AttributeTargets.Class)]
public class NodeIconAttribute : NodeViewAttribute
{
    public NodeIconAttribute(string svgPath) : base(typeof(IconNodeView), svgPath)
    {
    }
}
