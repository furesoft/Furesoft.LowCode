using Avalonia.PropertyGrid.Model.Extensions;

namespace Furesoft.LowCode;

public class SignalStorage
{
    public ObservableCollection<Signal> Signals { get; set; } = new();

    public void Register(string name, EmptyNode node)
    {
        if (!Signals.Contains(_ => _.Name == name))
        {
            Signals.Add(new(name));
        }

        Signals.First(_ => _.Name == name).RegisteredNodeHandlers.Add(node);
    }

    public async Task Trigger(string name, object value = null, CancellationToken token = default)
    {
        foreach (var handlerNode in Signals!.FirstOrDefault(_ => _.Name == name)?.RegisteredNodeHandlers!)
        {
            if (value != null)
            {
                handlerNode.Context.DefineConstant(name, handlerNode.Context.GlobalContext.WrapValue(value));
            }

            await handlerNode.Execute(token);
        }
    }

    public class Signal
    {
        public Signal(string name)
        {
            Name = name;
            RegisteredNodeHandlers = new();
        }

        public string Name { get; set; }
        public List<EmptyNode> RegisteredNodeHandlers { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
