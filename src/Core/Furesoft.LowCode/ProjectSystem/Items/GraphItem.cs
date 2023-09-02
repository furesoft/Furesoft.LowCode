using Furesoft.LowCode.Designer.Services.Serializing;
using Newtonsoft.Json;

namespace Furesoft.LowCode.ProjectSystem.Items;

public class GraphItem : ProjectItem
{
    [JsonIgnore] public NodeSerializer Serializer = new(typeof(ObservableCollection<>));

    public GraphItem(string name, IDrawingNode drawing, GraphProps graphProps) : base(name)
    {
        Id = Guid.NewGuid().ToString();
        Drawing = drawing;
        Props = graphProps;
    }

    public IDrawingNode Drawing { get; set; }
    public GraphProps Props { get; set; }

    public override string ToString()
    {
        return Serializer.Serialize(Drawing);
    }
}
