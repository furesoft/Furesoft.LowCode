using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Avalonia.Controls;
using NodeEditor.Model;
using NodeEditor.Mvvm;
using NodeEditorDemo.Core.NodeBuilding;
using INodeFactory = NodeEditor.Model.INodeFactory;

namespace NodeEditorDemo.Core;

public class NodeFactory : INodeFactory
{
    private readonly List<DynamicNode> _dynamicNodes = new();

    public void AddDynamicNode(DynamicNode node)
    {
        _dynamicNodes.Add(node);
    }

    public static CustomNodeViewModel CreateNode(string name, IEnumerable<(string name, PinAlignment alignment)> pins,
        (double x, double y) position, double width = 60,
        double height = 60, Control nodeView = null,
        double pinSize = 15)
    {
        var leftPins = pins.Where(_ => _.alignment == PinAlignment.Left).ToArray();
        var rightPins = pins.Where(_ => _.alignment == PinAlignment.Right).ToArray();
        var topPins = pins.Where(_ => _.alignment == PinAlignment.Top).ToArray();
        var bottomPins = pins.Where(_ => _.alignment == PinAlignment.Bottom).ToArray();

        var maxPinTopBottom = Math.Max(topPins.Length, bottomPins.Length);
        var maxPinLeftRight = Math.Max(leftPins.Length, rightPins.Length);

        (width, height) = RecalculateBoundsWithMargin((width, height), pinSize, maxPinTopBottom, maxPinLeftRight);
        nodeView ??= new DefaultNodeView();

        var viewModel = BaseNodeFactory.CreateViewModel(null, position, (width, height));

        AddPins(pinSize, topPins, viewModel, i => (CalculateSinglePin(width, topPins.Length, i), 0));
        AddPins(pinSize, bottomPins, viewModel, i => (CalculateSinglePin(width, bottomPins.Length, i), height));

        AddPins(pinSize, leftPins, viewModel, i => (0, CalculateSinglePin(height, leftPins.Length, i)));
        AddPins(pinSize, rightPins, viewModel, i => (width, CalculateSinglePin(height, rightPins.Length, i)));

        nodeView!.DataContext = viewModel.Content;

        viewModel.Content = nodeView;
        viewModel.Name = name;

        return viewModel;
    }

    public static CustomNodeViewModel CreateNode(VisualNode visualNode, (double x, double y) position,
        double width = 60,
        double height = 60,
        double pinSize = 15)
    {
        var pinData = visualNode.GetType().GetProperties()
            .Where(_ => _.GetCustomAttribute<PinAttribute>() != null)
            .Select(prop => (prop.GetCustomAttribute<PinAttribute>(), prop));

        var pins =
            from pin in pinData
            select (pin.Item1.Name ?? pin.prop.Name, pin.Item1.Alignment);

        Control nodeView = null;

        var nodeViewAttribute = visualNode.GetType().GetCustomAttribute<NodeViewAttribute>();
        if (nodeViewAttribute != null)
        {
            nodeView = (Control)Activator.CreateInstance(nodeViewAttribute.Type);

            if (nodeView.MinHeight > height)
            {
                height = nodeView.MinHeight;
            }

            if (nodeView.MinWidth > width)
            {
                width = nodeView.MinWidth;
            }
        }

        return CreateNode(visualNode.Label, pins, position, width, height, nodeView, pinSize);
    }

    private static (double, double) RecalculateBoundsWithMargin((double width, double height) size, double pinSize,
        int maxPinTopBottom,
        int maxPinLeftRight)
    {
        size.width = Math.Max(size.width, maxPinTopBottom * (2 + pinSize) * 1.6);
        size.height = Math.Max(size.height, maxPinLeftRight * (2 + pinSize) * 1.6);

        return (size.width, size.height);
    }

    private static double CalculateSinglePin(double width, int pinCount, int i)
    {
        return width / (pinCount + 1) * (i + 1);
    }

    private static void AddPins(double pinSize, IReadOnlyList<(string name, PinAlignment alignment)> pins,
        CustomNodeViewModel viewModel, Func<int, (double, double)> positionMapper)
    {
        for (int i = 0; i < pins.Count; i++)
        {
            var pin = pins[i];

            (double baseX, double baseY) = positionMapper(i);

            viewModel.AddPin(baseX, baseY, pinSize, pinSize, pin.alignment, pin.name);
        }
    }

    //ToDo: Refactor
    public IList<INodeTemplate> CreateTemplates()
    {
        var templates = new List<INodeTemplate>();
        var categories = new Dictionary<string, NodeCategory>();

        var nodes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(x => typeof(VisualNode).IsAssignableFrom(x) && x.IsClass)
            .Where(x => x.Name != nameof(VisualNode));

        var normalNodes = nodes.Where(_ => !typeof(INodeFactory).IsAssignableFrom(_) && _ != typeof(DynamicNode))
            .Select(type => (VisualNode)Activator.CreateInstance(type));
        var factoryNodes = nodes.Where(_ => typeof(INodeFactory).IsAssignableFrom(_))
            .Select(type => (VisualNode)Activator.CreateInstance(type));

        foreach (var node in normalNodes)
        {
            var ignoreAttribute = node.GetType().GetCustomAttribute<IgnoreTemplateAttribute>();
            if (ignoreAttribute != null)
            {
                continue;
            }

            string category = "General";
            var categoryAttribute = node.GetType().GetCustomAttribute<NodeCategoryAttribute>();
            if (categoryAttribute != null)
            {
                category = categoryAttribute.Category;
            }

            if (!categories.ContainsKey(category))
            {
                categories.Add(category,
                    new() {Name = category, Templates = new ObservableCollection<INodeTemplate>()});
            }

            templates.Add(new NodeTemplateViewModel
            {
                Title = node.Label, Template = CreateNode(node, (0, 0)), Preview = CreateNode(node, (0, 0))
            });
        }

        foreach (var factoryNode in factoryNodes)
        {
            var factory = (INodeFactory)factoryNode;

            string category = "General";
            var categoryAttribute = factoryNode.GetType().GetCustomAttribute<NodeCategoryAttribute>();
            if (categoryAttribute != null)
            {
                category = categoryAttribute.Category;
            }

            if (!categories.ContainsKey(category))
            {
                categories.Add(category,
                    new() {Name = category, Templates = new ObservableCollection<INodeTemplate>()});
            }

            foreach (var node in factory.CreateTemplates())
            {
                templates.Add(node);
            }
        }

        foreach (var dynamicNode in _dynamicNodes)
        {
            var pins = dynamicNode.Pins.Select(_ => (_.Key, _.Value));

            templates.Add(new NodeTemplateViewModel
            {
                Title = dynamicNode.Label,
                Template = CreateNode(dynamicNode.Label, pins, (0, 0), nodeView: dynamicNode.View),
                Preview = CreateNode(dynamicNode.Label, pins, (0, 0), nodeView: dynamicNode.View)
            });
        }

        return templates;
    }

    public IDrawingNode CreateDrawing(string name = null)
    {
        return BaseNodeFactory.CreateDrawing(name);
    }

    public void PrintNetList(IDrawingNode? drawing)
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
