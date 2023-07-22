using System;

namespace Furesoft.LowCode.Core.NodeBuilding;

[AttributeUsage(AttributeTargets.Class)]
public class NodeViewAttribute : Attribute
{
    public NodeViewAttribute(Type type)
    {
        Type = type;
    }

    public Type Type { get; set; }
}