using System.ComponentModel;

namespace Furesoft.LowCode.Designer.Core;

internal class DynamicNodePropertyDescriptor : PropertyDescriptor
{

    public DynamicNodePropertyDescriptor(MemberDescriptor descr) : base(descr)
    {
    }

    public DynamicNodePropertyDescriptor(MemberDescriptor descr, Attribute[] attrs) : base(descr, attrs)
    {
    }

    public DynamicNodePropertyDescriptor(string name, Attribute[] attrs, Type propertyType) : base(name, attrs)
    {
        PropertyType = propertyType;
    }

    public override bool CanResetValue(object component)
    {
        return true;
    }

    public override object GetValue(object component)
    {
        if (component is DynamicNode dn)
        {
            if (dn.Properties.TryGetValue(Name, out var value))
            {
                return value;
            }
        }

        return null;
    }

    public override void ResetValue(object component)
    {
        if (component is DynamicNode dn)
        {
            dn.Properties[Name] = default;
        }
    }

    public override void SetValue(object component, object value)
    {
        if (component is DynamicNode dn)
        {
            dn.Properties[Name] = value;
        }
    }

    public override bool ShouldSerializeValue(object component)
    {
        return true;
    }

    public override Type ComponentType { get; } = typeof(DynamicNode);
    public override bool IsReadOnly => false;
    public override Type PropertyType { get; }
}
