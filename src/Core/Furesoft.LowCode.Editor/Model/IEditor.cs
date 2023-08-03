namespace Furesoft.LowCode.Editor.Model;

public interface IEditor
{
    IList<INodeTemplate> Templates { get; set; }
    IDrawingNode Drawing { get; set; }
}
