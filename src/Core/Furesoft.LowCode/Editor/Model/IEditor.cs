namespace Furesoft.LowCode.Editor.Model;

public interface IEditor
{
    ObservableCollection<object> Templates { get; set; }
    IDrawingNode Drawing { get; set; }
}
