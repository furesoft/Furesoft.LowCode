using PropertyModels.Extensions;

namespace Furesoft.LowCode;

public class SignalStorage
{
    public ObservableCollection<Signal> Signals { get; } = new();

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

    public class Signal(string name)
    {
        public string Name { get; } = name;
        public List<EmptyNode> RegisteredNodeHandlers { get; } = new();

        public override string ToString()
        {
            return Name;
        }
    }
}
