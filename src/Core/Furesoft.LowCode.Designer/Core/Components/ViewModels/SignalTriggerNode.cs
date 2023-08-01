using System.ComponentModel;
using System.Runtime.Serialization;
using Avalonia.PropertyGrid.Model.Collections;

namespace Furesoft.LowCode.Designer.Core.Components.ViewModels;

[Description("Triggers the selected signal with the specified argument")]
[DataContract(IsReference = true)]
[NodeCategory]
public class SignalTriggerNode : InputOutputNode
{
    public SignalTriggerNode() : base("Trigger")
    {
        /* VisualNode.Signals.Signals.CollectionChanged += (sender, args) =>
         {
             var argsNewItem = (SignalStorage.Signal)args.NewItems[0];
             Signals.Add(argsNewItem);
         };
         
         Signals = new(VisualNode.Signals.Signals);
         
         Signals.Add(new("OnEnter"));
         Signals.Add(new("OnLeaver"));
         Signals.Add(new("OnSomething"));
 
         Signals.SelectedValue = Signals.First();*/
    }

    public SelectableList<SignalStorage.Signal> Signals { get; set; }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        //await VisualNode.Signals.Trigger(Signals.SelectedValue.Name, token: cancellationToken);

        await ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }
}
