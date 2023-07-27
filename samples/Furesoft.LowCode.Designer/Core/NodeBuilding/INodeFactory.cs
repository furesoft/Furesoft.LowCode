using System.Collections.Generic;

namespace Furesoft.LowCode.Designer.Core.NodeBuilding;

public interface INodeFactory
{
    IEnumerable<VisualNode> Create();
}