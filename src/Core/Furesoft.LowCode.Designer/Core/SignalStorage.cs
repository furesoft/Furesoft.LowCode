﻿using System.Collections.ObjectModel;
using Avalonia.PropertyGrid.Model.Extensions;
using NiL.JS.Core;

namespace Furesoft.LowCode.Designer.Core;

public class SignalStorage
{
    public class Signal
    {
        public Signal(string name)
        {
            Name = name;
            RegisteredNodeHandlers = new();
        }

        public string Name { get; set; }
        public List<VisualNode> RegisteredNodeHandlers { get; set; }

        public override string ToString() => Name;
    }

    public ObservableCollection<Signal> Signals { get; set; } = new();

    public void Register(string name, VisualNode node)
    {
        if (!Signals.Contains(_ => _.Name == name))
        {
            Signals.Add(new(name));
        }
        
        Signals.First(_=> _.Name == name).RegisteredNodeHandlers.Add(node);
    }

    public async Task Trigger(string name, object value = null, CancellationToken token = default)
    {
        foreach (var handlerNode in Signals!.FirstOrDefault(_=> _.Name == name)?.RegisteredNodeHandlers!)
        {
            if (value != null)
            {
                handlerNode.Context.DefineConstant(name, JSValue.Wrap(value));
            }
            
            await handlerNode.Execute(token);
        }
    }
}