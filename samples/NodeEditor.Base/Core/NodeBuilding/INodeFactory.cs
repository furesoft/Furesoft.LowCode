using System.Collections.Generic;

namespace NodeEditorDemo.Core.NodeBuilding;

public interface INodeFactory
{
    IEnumerable<VisualNode> Create();
}