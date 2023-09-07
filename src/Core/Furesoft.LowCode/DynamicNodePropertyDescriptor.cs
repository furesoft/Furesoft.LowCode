using System.ComponentModel;

namespace Furesoft.LowCode;

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

    public override Type ComponentType { get; } = typeof(DynamicNode);
    public override bool IsReadOnly => false;
    public override Type PropertyType { get; }

    public override bool CanResetValue(object component)
    {
        return true;
    }

    public override object GetValue(object component)
    {
        if (component is DynamicNode dn)
        {
            var prop = dn.Properties.FirstOrDefault(p => p.Name == Name);

            if (prop != null)
            {
                return prop.Value;
            }
        }

        return null;
    }

    public override void ResetValue(object component)
    {
        if (component is DynamicNode dn)
        {
            var prop = dn.Properties.FirstOrDefault(p => p.Name == Name);

            if (prop != null)
            {
                // Assuming default value for type can be null
                prop.Value = default;
            }
        }
    }

    public override void SetValue(object component, object value)
    {
        if (component is DynamicNode dn)
        {
            var prop = dn.Properties.FirstOrDefault(p => p.Name == Name);

            if (prop != null)
            {
                prop.Value = value;
            }
        }
    }

    public override bool ShouldSerializeValue(object component)
    {
        return true;
    }
}
