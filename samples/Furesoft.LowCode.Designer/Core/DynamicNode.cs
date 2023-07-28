﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Furesoft.LowCode.Designer.Core.NodeBuilding;
using Furesoft.LowCode.Editor.Model;
using Furesoft.LowCode.Editor.MVVM;

namespace Furesoft.LowCode.Designer.Core;

public class DynamicNode : VisualNode, ICustomTypeDescriptor
{
    public readonly Dictionary<string, (PinAlignment, PinMode)> Pins = new();
    public readonly Dictionary<string, object> Properties = new();
    
    [Browsable(false)] public Control View { get; set; }

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

    public void AddPin(string name, PinAlignment alignment, PinMode mode)
    {
        Pins.Add(name, (alignment, mode));
    }

    public override Task Execute(CancellationToken cancellationToken)
    {
        SetOutVariable(Properties["out"].ToString(), true);

        return Task.CompletedTask;
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
        var result = new List<PropertyDescriptor>();

        foreach (var property in Properties)
        {
            var descriptor = new DynamicNodePropertyDescriptor(property.Key, null, property.Value.GetType());
            
            result.Add(descriptor);
        }

        return new(result.ToArray());
    }

    public object GetPropertyOwner(PropertyDescriptor pd)
    {
        return this;
    }

    #endregion
}