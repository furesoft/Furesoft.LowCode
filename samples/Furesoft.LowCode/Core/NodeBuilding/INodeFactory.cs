using System.Collections.Generic;

namespace Furesoft.LowCode.Core.NodeBuilding;

public interface INodeFactory
{
    IEnumerable<VisualNode> Create();
}