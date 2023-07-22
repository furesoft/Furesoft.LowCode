using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using NodeEditorDemo.Core.NodeBuilding;
using NodeEditorDemo.ViewModels;

namespace NodeEditorDemo.Core;

[DataContract(IsReference = true)]
public abstract class VisualNode : ViewModelBase
{
    private string _label;
    public Evaluator Evaluator;

    public VisualNode(string label)
    {
        Label = label;
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string Label
    {
        get => _label;
        set => SetProperty(ref _label, value);
    }

    public abstract void Execute();

    protected void ExecutePin(IOutputPin pin, [CallerArgumentExpression("pin")] string pinMembername = null)
    {
        var pinName = GetPinName(pinMembername);

        var connections =
            from connection in Drawing.Connectors
            where ((CustomNodeViewModel)connection.Start.Parent).DefiningNode == this
            select connection;

        var pinConnections = connections.Where(_ => _.Start.Name == pinName);

        foreach (var pinConnection in pinConnections)
        {
            var parent = (CustomNodeViewModel)pinConnection.End.Parent;

            parent.DefiningNode.Evaluator = Evaluator;
            parent.DefiningNode.Drawing = Drawing;
            
            parent.DefiningNode.Execute();
        }
    }

    private string GetPinName(string propertyName)
    {
        var propInfo = GetType().GetProperty(propertyName);

        var attr = propInfo.GetCustomAttribute<PinAttribute>();

        if (attr == null)
        {
            return propInfo.Name;
        }

        return attr.Name;
    }
}
