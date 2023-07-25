using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Avalonia.Controls;
using Furesoft.LowCode.Core.Components.Views;
using Furesoft.LowCode.Core.NodeBuilding;
using NodeEditor.Model;
using NodeEditor.Mvvm;
using INodeFactory = NodeEditor.Model.INodeFactory;

namespace Furesoft.LowCode.Core;

public partial class NodeFactory : INodeFactory
{
    private const int PinSize = 15;

    private readonly List<DynamicNode> _dynamicNodes = new();

    public List<string> SearchPaths = new() {"."};

    public void AddDynamicNode(DynamicNode node)
    {
        _dynamicNodes.Add(node);
    }

    private static CustomNodeViewModel CreateNode(VisualNode node, IEnumerable<KeyValuePair<string, PinAlignment>> pins,
        (double x, double y) position, double width = 60, double height = 60, Control nodeView = null)
    {
        var leftPins = pins.Where(_ => _.Value == PinAlignment.Left).ToArray();
        var rightPins = pins.Where(_ => _.Value == PinAlignment.Right).ToArray();
        var topPins = pins.Where(_ => _.Value == PinAlignment.Top).ToArray();
        var bottomPins = pins.Where(_ => _.Value == PinAlignment.Bottom).ToArray();

        var maxPinTopBottom = Math.Max(topPins.Length, bottomPins.Length);
        var maxPinLeftRight = Math.Max(leftPins.Length, rightPins.Length);

        (width, height) = RecalculateBoundsWithMargin((width, height), maxPinTopBottom, maxPinLeftRight);
        nodeView ??= new DefaultNodeView();

        var viewModel = CreateViewModel(null, position, (width, height));

        AddPins(PinSize, topPins, viewModel, i => (CalculateSinglePin(width, topPins.Length, i), 0));
        AddPins(PinSize, bottomPins, viewModel, i => (CalculateSinglePin(width, bottomPins.Length, i), height));

        AddPins(PinSize, leftPins, viewModel, i => (0, CalculateSinglePin(height, leftPins.Length, i)));
        AddPins(PinSize, rightPins, viewModel, i => (width, CalculateSinglePin(height, rightPins.Length, i)));

        nodeView!.DataContext = node;

        viewModel.Content = nodeView;
        viewModel.Name = node.Label;
        viewModel.DefiningNode = node;

        var attributes = TypeDescriptor.GetAttributes(node);
        var descriptionAttribute = attributes.OfType<DescriptionAttribute>().FirstOrDefault();
        if (descriptionAttribute != null)
        {
            node.Description = descriptionAttribute.Description;
        }
        
        var categoryAttribute = attributes.OfType<CategoryAttribute>().FirstOrDefault();
        if (categoryAttribute != null)
        {
            viewModel.Category = categoryAttribute.Category;
        }

        return viewModel;
    }

    private static CustomNodeViewModel CreateNode(VisualNode visualNode, (double x, double y) position,
        double width = 60, double height = 60)
    {
        var pinData =
            from prop in visualNode.GetType().GetProperties()
            where prop.GetCustomAttribute<PinAttribute>() != null
            select (prop.GetCustomAttribute<PinAttribute>(), prop);

        var pins =
            from pin in pinData
            select new KeyValuePair<string, PinAlignment>(pin.Item1.Name ?? pin.prop.Name, pin.Item1.Alignment);

        Control nodeView = null;

        var nodeViewAttribute = visualNode.GetType().GetCustomAttribute<NodeViewAttribute>();
        if (nodeViewAttribute != null)
        {
            nodeView = (Control)Activator.CreateInstance(nodeViewAttribute.Type);

            if (nodeView!.MinHeight > height)
            {
                height = nodeView.MinHeight;
            }

            if (nodeView.MinWidth > width)
            {
                width = nodeView.MinWidth;
            }

            nodeView.Tag = nodeViewAttribute.Parameter;
        }

        return CreateNode(visualNode, pins, position, width, height, nodeView);
    }

    private static (double, double) RecalculateBoundsWithMargin((double width, double height) size,
        int maxPinTopBottom, int maxPinLeftRight)
    {
        double Calculate(int max)
        {
            return max * (2 + PinSize) * 1.6;
        }

        size.width = Math.Max(size.width, Calculate(maxPinTopBottom));
        size.height = Math.Max(size.height, Calculate(maxPinLeftRight));

        return (size.width, size.height);
    }

    public IList<INodeTemplate> CreateTemplates()
    {
        bool IsVisualNode(Type type)
        {
            return typeof(VisualNode).IsAssignableFrom(type) && type.IsClass && type.Name != nameof(VisualNode);
        }

        var templates = new List<INodeTemplate>();

        var nodeTypes =
            from folder in SearchPaths
            from assemblyFile in Directory.GetFiles(folder, "*.dll")
            let assembly = Assembly.LoadFrom(assemblyFile)
            from type in assembly.GetTypes()
            where IsVisualNode(type)
            select type;

        CreateNormalNodeTemplates(nodeTypes, templates);

        CreateFactoryNodeTemplates(nodeTypes, templates);

        CreateDynamicNodeTemplates(templates);

        return templates;
    }

    private void CreateDynamicNodeTemplates(List<INodeTemplate> templates)
    {
        foreach (var dynamicNode in _dynamicNodes)
        {
            var pins = dynamicNode.Pins;

            templates.Add(new NodeTemplateViewModel
            {
                Title = dynamicNode.Label,
                Template = CreateNode(dynamicNode, pins, (0, 0), nodeView: dynamicNode.View),
                Preview = CreateNode(dynamicNode, pins, (0, 0), nodeView: dynamicNode.View)
            });
        }
    }

    private static void CreateFactoryNodeTemplates(IEnumerable<Type> nodes, List<INodeTemplate> templates)
    {
        bool IsFactoryNode(Type node)
        {
            return typeof(INodeFactory).IsAssignableFrom(node);
        }

        var factoryNodes = from node in nodes
            where IsFactoryNode(node)
            select (VisualNode)Activator.CreateInstance(node);

        foreach (var factoryNode in factoryNodes)
        {
            var factory = factoryNode as INodeFactory;

            templates.AddRange(factory!.CreateTemplates());
        }
    }

    private static void CreateNormalNodeTemplates(IEnumerable<Type> nodes, List<INodeTemplate> templates)
    {
        bool IsNormalNode(Type node)
        {
            return !typeof(INodeFactory).IsAssignableFrom(node) && node != typeof(DynamicNode);
        }

        var normalNodes =
            from node in nodes
            where IsNormalNode(node)
            select (VisualNode)Activator.CreateInstance(node);

        foreach (var node in normalNodes)
        {
            var ignoreAttribute = node.GetType().GetCustomAttribute<IgnoreTemplateAttribute>();
            if (ignoreAttribute != null)
            {
                continue;
            }

            templates.Add(new NodeTemplateViewModel
            {
                Title = node.Label, Template = CreateNode(node, (0, 0)), Preview = CreateNode(node, (0, 0))
            });
        }
    }

    public void PrintNetList(IDrawingNode drawing)
    {
        if (drawing?.Connectors is null || drawing?.Nodes is null)
        {
            return;
        }

        foreach (var connector in drawing.Connectors)
        {
            if (connector.Start is { } start && connector.End is { } end)
            {
                Debug.WriteLine(
                    $"{start.Parent?.Content.GetType().Name}:{start.GetType().Name} -> {end.Parent?.Content.GetType().Name}:{end.GetType().Name}");
            }
        }
    }
}
