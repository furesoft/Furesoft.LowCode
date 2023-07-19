using System;

namespace NodeEditorDemo.Core.NodeBuilding;

[AttributeUsage(AttributeTargets.Class)]
public class NodeCategoryAttribute : Attribute
{
    public NodeCategoryAttribute(string category)
    {
        Category = category;
    }

    public string Category { get; set; }
}