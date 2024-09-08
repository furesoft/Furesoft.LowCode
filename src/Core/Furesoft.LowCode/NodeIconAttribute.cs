using Furesoft.LowCode.NodeViews;

namespace Furesoft.LowCode;

[AttributeUsage(AttributeTargets.Class)]
public class NodeIconAttribute(string svgPath) : NodeViewAttribute(typeof(IconNodeView), svgPath);
