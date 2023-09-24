using NiL.JS.Core;

namespace Furesoft.LowCode;

public static class SignalStorage
{
    private static List<Signal> Signals { get; set; } = new();

    public static void Register(string name, ICallable callback)
    {
        if (!Signals.Any(_ => _.Name == name))
        {
            Signals.Add(new(name));
        }

        Signals.First(_ => _.Name == name).RegisteredCallableHandlers.Add(callback);
    }

    public static void Trigger(string name, object value = null)
    {
        var args = new Arguments(Context.CurrentGlobalContext);

        foreach (var handler in Signals!.FirstOrDefault(_ => _.Name == name)?.RegisteredCallableHandlers!)
        {
            if (value != null)
            {
                args.Add(value);
            }

            handler.Call(null, args);
        }
    }

    public class Signal
    {
        public Signal(string name)
        {
            Name = name;
            RegisteredCallableHandlers = new();
        }

        public string Name { get; set; }
        public List<ICallable> RegisteredCallableHandlers { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
