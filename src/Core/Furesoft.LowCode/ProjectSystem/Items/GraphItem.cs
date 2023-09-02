using Furesoft.LowCode.Designer.Services.Serializing;
using Newtonsoft.Json;

namespace Furesoft.LowCode.ProjectSystem.Items;

public class GraphItem : ProjectItem
{
    [JsonIgnore] public NodeSerializer Serializer = new(typeof(ObservableCollection<>));

    public GraphItem(string id, IDrawingNode drawing, GraphProps graphProps) : base(null)
    {
        Id = id;
        Drawing = drawing;
        Name = drawing.Name;
        Props = graphProps;
    }

    public IDrawingNode Drawing { get; set; }
    public GraphProps Props { get; set; }

    public override string ToString()
    {
        return Serializer.Serialize(this);
    }
}
