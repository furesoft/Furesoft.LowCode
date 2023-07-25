using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Avalonia.PropertyGrid.Model.Extensions;
using Furesoft.LowCode.Core.NodeBuilding;
using Furesoft.LowCode.ViewModels;
using NiL.JS.Core;
using NodeEditor.Model;

namespace Furesoft.LowCode.Core;

[DataContract(IsReference = true)]
public abstract class VisualNode : ViewModelBase, ICustomTypeDescriptor
{
    private string _label;
    private string _description;
    internal Evaluator _evaluator;
    protected Context Context => _evaluator?.Context;

    public VisualNode(string label)
    {
        Label = label;
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    [Browsable(false)]
    public string Label
    {
        get => _label;
        set => SetProperty(ref _label, value);
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    [Browsable(false)]
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    /// <summary>
    /// Gets the previously executed node
    /// </summary>
    [Browsable(false)]
    public VisualNode PreviousNode { get; set; }
    
    public abstract Task Execute();

    protected async Task ContinueWith(IOutputPin pin, [CallerArgumentExpression("pin")] string pinMembername = null)
    {
        var pinName = GetPinName(pinMembername);
        var connections = GetConnections();
        var pinViewModel = GetPinViewModel(pinName);

        var pinConnections = GetPinConnections(connections, pinViewModel);

        foreach (var pinConnection in pinConnections)
        {
            CustomNodeViewModel parent;

            if (pinConnection.Start.Name == pinName)
            {
                parent = pinConnection.End.Parent as CustomNodeViewModel;
            }
            else if (pinConnection.End.Name == pinName)
            {
                parent = pinConnection.Start.Parent as CustomNodeViewModel;
            }
            else
            {
                continue;
            }

            parent.DefiningNode._evaluator = _evaluator;
            parent.DefiningNode.Drawing = Drawing;
            parent.DefiningNode.PreviousNode = this;

            await parent.DefiningNode.Execute();
        }
    }

    private static IEnumerable<IConnector> GetPinConnections(IEnumerable<IConnector> connections, IPin pinViewModel)
    {
        return from conn in connections
            where conn.Start == pinViewModel || conn.End == pinViewModel
            select conn;
    }

    private IPin GetPinViewModel(string pinName)
    {
        return (from node in Drawing.Nodes
            where ((CustomNodeViewModel)node).DefiningNode == this
            from pinn in node.Pins
            where pinn.Name == pinName
            select pinn).FirstOrDefault();
    }

    private IEnumerable<IConnector> GetConnections()
    {
        return from connection in Drawing.Connectors
            where ((CustomNodeViewModel)connection.Start.Parent).DefiningNode == this
                  || ((CustomNodeViewModel)connection.End.Parent).DefiningNode == this
            select connection;
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

    protected T Evaluate<T>(string src)
    {
        return _evaluator.Evaluate<T>(src);
    }

    protected void SetOutVariable(string name, object value)
    {
        Context.GetVariable(name).Assign(JSValue.Wrap(value));
    }

    public string GetCallStack()
    {
        var sb = new StringBuilder();
        
        sb.AppendLine($"{Label}:");
        foreach (PropertyDescriptor value in GetProperties())
        {
            sb.AppendLine($"\t{value.Name}: {value.GetValue(this)}");
        }

        sb.AppendLine(PreviousNode?.GetCallStack());
        
        return sb.ToString();
    }

    #region Custom Type Descriptor Interfaces

    public AttributeCollection GetAttributes()
    {
        return TypeDescriptor.GetAttributes(this, true);
    }

    public string GetClassName()
    {
        return TypeDescriptor.GetClassName(this, true);
    }

    public string GetComponentName()
    {
        return TypeDescriptor.GetComponentName(this, true);
    }

    public TypeConverter GetConverter()
    {
        return TypeDescriptor.GetConverter(this, true);
    }

    public EventDescriptor GetDefaultEvent()
    {
        return TypeDescriptor.GetDefaultEvent(this, true);
    }

    public PropertyDescriptor GetDefaultProperty()
    {
        return TypeDescriptor.GetDefaultProperty(this, true);
    }

    public object GetEditor(Type editorBaseType)
    {
        return TypeDescriptor.GetEditor(this, editorBaseType, true);
    }

    public EventDescriptorCollection GetEvents()
    {
        return TypeDescriptor.GetEvents(this, true);
    }

    public EventDescriptorCollection GetEvents(Attribute[] attributes)
    {
        return TypeDescriptor.GetEvents(this, attributes, true);
    }

    public PropertyDescriptorCollection GetProperties()
    {
        return GetProperties(null);
    }

    public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
    {
        return new(
            (from PropertyDescriptor property in TypeDescriptor.GetProperties(this, true)
                where property.PropertyType != typeof(IInputPin) && property.PropertyType != typeof(IOutputPin)
                let attribute = property.Attributes.OfType<BrowsableAttribute>().FirstOrDefault()
                where attribute == null || attribute.Browsable
                select TypeDescriptor.CreateProperty(GetType(), property.Name, property.PropertyType)).ToArray());
    }

    public object GetPropertyOwner(PropertyDescriptor pd)
    {
        return this;
    }

    #endregion
}
