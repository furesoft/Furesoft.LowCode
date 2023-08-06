using System.ComponentModel;
using Avalonia.Controls;

namespace Furesoft.LowCode;

public class DynamicNode : EmptyNode, ICustomTypeDescriptor
{
    public readonly Dictionary<string, (PinAlignment, PinMode, bool)> Pins = new();
    public readonly Dictionary<string, object> Properties = new();

    public DynamicNode(string label, Control view = null) : base(label)
    {
        View = view;

        TypeDescriptor.AddAttributes(this, new DescriptionAttribute("A simple dynamic node"));
        TypeDescriptor.AddAttributes(this, new NodeCategoryAttribute("Base"));

        Properties.Add("Customproperty1", 5);
        Properties.Add("Customproperty2", "Hello");
        Properties.Add("Customproperty3", DateTime.Now);
        Properties.Add("out", string.Empty);
    }

    [Browsable(false)] public Control View { get; set; }

    public void AddPin(string name, PinAlignment alignment, PinMode mode, bool multipleConnections = false)
    {
        Pins.Add(name, (alignment, mode, multipleConnections));
    }

    public override Task Execute(CancellationToken cancellationToken)
    {
        SetOutVariable(Properties["out"].ToString(), true);

        return Task.CompletedTask;
    }

    #region Custom Type Descriptor Interfaces

    public new AttributeCollection GetAttributes()
    {
        return TypeDescriptor.GetAttributes(this, true);
    }

    public new string GetClassName()
    {
        return TypeDescriptor.GetClassName(this, true);
    }

    public new string GetComponentName()
    {
        return TypeDescriptor.GetComponentName(this, true);
    }

    public new TypeConverter GetConverter()
    {
        return TypeDescriptor.GetConverter(this, true);
    }

    public new EventDescriptor GetDefaultEvent()
    {
        return TypeDescriptor.GetDefaultEvent(this, true);
    }

    public new PropertyDescriptor GetDefaultProperty()
    {
        return TypeDescriptor.GetDefaultProperty(this, true);
    }

    public new object GetEditor(Type editorBaseType)
    {
        return TypeDescriptor.GetEditor(this, editorBaseType, true);
    }

    public new EventDescriptorCollection GetEvents()
    {
        return TypeDescriptor.GetEvents(this, true);
    }

    public new EventDescriptorCollection GetEvents(Attribute[] attributes)
    {
        return TypeDescriptor.GetEvents(this, attributes, true);
    }

    public new PropertyDescriptorCollection GetProperties()
    {
        return GetProperties(null);
    }

    public new PropertyDescriptorCollection GetProperties(Attribute[] attributes)
    {
        var result = new List<PropertyDescriptor>();

        foreach (var property in Properties)
        {
            var descriptor = new DynamicNodePropertyDescriptor(property.Key, null, property.Value.GetType());

            result.Add(descriptor);
        }

        return new(result.ToArray());
    }

    public new object GetPropertyOwner(PropertyDescriptor pd)
    {
        return this;
    }

    #endregion
}
