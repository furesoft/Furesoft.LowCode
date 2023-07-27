using System.Collections.Generic;

namespace Furesoft.LowCode.Editor.Model;

public interface INodeFactory
{
    IList<INodeTemplate> CreateTemplates();
    IDrawingNode CreateDrawing(string? name = null);
}
