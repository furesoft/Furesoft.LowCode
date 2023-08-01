using System.ComponentModel;
using System.Runtime.Serialization;
using Avalonia.PropertyGrid.Model.Collections;

namespace Furesoft.LowCode.Designer.Core.Components.ViewModels;

[Description("Executes when a Signal was triggered")]
[DataContract(IsReference = true)]
[NodeCategory]
public class SignalNode : VisualNode
{
    public SignalNode() : base("Signal")
    {
        /*
        _evaluator.Signals.Signals.CollectionChanged += (sender, args) =>
        {
            var argsNewItem = (SignalStorage.Signal)args.NewItems[0];
            Signals.Add(argsNewItem);
        };
        
        Signals = new(VisualNode.Signals.Signals);
        
        Signals.Add(new("OnEnter"));
        Signals.Add(new("OnLeaver"));
        Signals.Add(new("OnSomething"));

        Signals.SelectedValue = Signals.First();
    */
    }
    
    public SelectableList<SignalStorage.Signal> Signals { get; set; }
    
    
    [Pin("Flow", PinAlignment.Bottom)]
    public IOutputPin OutputFlowNode { get; set; }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        // VisualNode.Signals.Register(Signals.SelectedValue.Name, this);

        await ContinueWith(OutputFlowNode, cancellationToken: cancellationToken);
    }

}
