using System.ComponentModel;
using Furesoft.LowCode.Designer.Layout.Models.Tools.Parameters;
using PropDescriptor = Furesoft.LowCode.Designer.Layout.Models.Tools.Parameters.PropertyDescriptor;
using PropertyDescriptor = System.ComponentModel.PropertyDescriptor;

namespace Furesoft.LowCode;

public class DynamicNode : EmptyNode, ICustomTypeDescriptor
{
    public readonly List<PinDescriptor> Pins = new();
    public readonly List<PropDescriptor> Properties = new();

    private Func<DynamicNode, CancellationToken, Task> _action;

    public DynamicNode(string label, Control view = null) : base(label)
    {
        View = view;
    }

    [Browsable(false)] public Control View { get; set; }

    public void AddPin(string name, PinAlignment alignment, PinMode mode, bool multipleConnections = false)
    {
        Pins.Add(new(name, mode, alignment, multipleConnections));
    }

    protected Task ContinueWith(string pin, CancellationToken token)
    {
        return ContinueWith(null, token, pinMembername: pin);
    }

    public override Task Execute(CancellationToken cancellationToken)
    {
        return _action?.Invoke(this, cancellationToken);
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
            var descriptor = new DynamicNodePropertyDescriptor(property.Name, null, property.Type);
            descriptor.SetValue(this, property.Value);

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
