using System;

namespace Furesoft.LowCode.Core.NodeBuilding;

[AttributeUsage(AttributeTargets.Class)]
public class NodeCategoryAttribute : Attribute
{
    public NodeCategoryAttribute(string category)
    {
        Category = category;
    }

    public string Category { get; set; }
}