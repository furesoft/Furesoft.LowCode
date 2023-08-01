using System.ComponentModel;
using System.Reflection;
using Avalonia.Controls;
using Furesoft.LowCode.Designer.Core.Components.Views;
using Furesoft.LowCode.Editor.Model;
using Furesoft.LowCode.Editor.MVVM;

namespace Furesoft.LowCode.Designer.Core;

public partial class NodeFactory : INodeFactory
{
    private const int PinSize = 15;

    private readonly List<DynamicNode> _dynamicNodes = new();

    public readonly List<string> SearchPaths = new() {"."};

    public void AddDynamicNode(DynamicNode node)
    {
        _dynamicNodes.Add(node);
    }

    private static CustomNodeViewModel CreateNode(EmptyNode node,
        IEnumerable<(string Key, PinAlignment Value, PinMode mode, bool)> pins,
        (double x, double y) position, double width = 60, double height = 60, Control nodeView = null)
    {
        var leftPins =
            pins.Where(_ => _.Value == PinAlignment.Left).ToArray();
        var rightPins =
            pins.Where(_ => _.Value == PinAlignment.Right).ToArray();
        var topPins =
            pins.Where(_ => _.Value == PinAlignment.Top).ToArray();
        var bottomPins =
            pins.Where(_ => _.Value == PinAlignment.Bottom).ToArray();

        var maxPinTopBottom = Math.Max(topPins.Length, bottomPins.Length);
        var maxPinLeftRight = Math.Max(leftPins.Length, rightPins.Length);

        (width, height) = RecalculateBoundsWithMargin(node, (width, height), maxPinTopBottom, maxPinLeftRight);
        nodeView ??= new DefaultNodeView();

        var viewModel = CreateViewModel(node, position, (width, height));

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

    private static CustomNodeViewModel CreateNode(EmptyNode emptyNode, (double x, double y) position,
        double width = 60, double height = 60)
    {
        var pinData =
            from prop in emptyNode.GetType().GetProperties()
            where prop.GetCustomAttribute<PinAttribute>() != null
            select (prop.GetCustomAttribute<PinAttribute>(), prop);

        var pins =
            from pin in pinData
            select (pin.Item1.Name ?? pin.prop.Name,
                pin.Item1.Alignment,
                pin.prop.PropertyType == typeof(IOutputPin) ? PinMode.Output : PinMode.Input,
                pin.Item1.AllowMultipleConnections);

        Control nodeView = null;

        var nodeViewAttribute = emptyNode.GetAttribute<NodeViewAttribute>();
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

        return CreateNode(emptyNode, pins, position, width, height, nodeView);
    }

    private static (double, double) RecalculateBoundsWithMargin(EmptyNode node, (double width, double height) size,
        int maxPinTopBottom, int maxPinLeftRight)
    {
        double Calculate(int max)
        {
            return max * (2 + PinSize) * 1.6;
        }

        size.width = Max(size.width, Calculate(maxPinTopBottom), node.GetAttribute<NodeViewAttribute>()?.MinWidth);
        size.height = Max(size.height, Calculate(maxPinLeftRight), node.GetAttribute<NodeViewAttribute>()?.MinHeight);
        
        return (size.width, size.height);
    }

    private static double Max(params double?[] values)
    {
        return values.Max().GetValueOrDefault();
    }

    public IList<INodeTemplate> CreateTemplates()
    {
        bool IsVisualNode(Type type)
        {
            return typeof(EmptyNode).IsAssignableFrom(type) && type.IsClass && type.Name != nameof(EmptyNode);
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
            var pins = dynamicNode.Pins.Select(_ => (_.Key, _.Value.Item1, _.Value.Item2, _.Value.Item3));

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
            select Activator.CreateInstance(node);

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
            return !typeof(INodeFactory).IsAssignableFrom(node) && node != typeof(DynamicNode) && !node.IsAbstract;
        }

        var normalNodes =
            from node in nodes
            where IsNormalNode(node)
            select (EmptyNode)Activator.CreateInstance(node);

        templates.AddRange(from node in normalNodes
            let ignoreAttribute = node.GetAttribute<IgnoreTemplateAttribute>()
            where ignoreAttribute == null
            select new NodeTemplateViewModel
            {
                Title = node.Label, Template = CreateNode(node, (0, 0)), Preview = CreateNode(node, (0, 0))
            });
    }
}
