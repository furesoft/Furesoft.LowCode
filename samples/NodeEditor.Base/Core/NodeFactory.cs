using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Avalonia.Controls;
using NodeEditor.Model;
using NodeEditor.Mvvm;
using NodeEditorDemo.Core.Components.ViewModels;
using NodeEditorDemo.Core.NodeBuilding;
using INodeFactory = NodeEditor.Model.INodeFactory;

namespace NodeEditorDemo.Core;

public class CustomNodeViewModel : NodeViewModel
{
    public bool IsRemovable { get; set; } = true;
    public bool IsMovable { get; set; } = true;

    public override bool CanRemove()
    {
        return IsRemovable;
    }

    public override bool CanMove()
    {
        return IsMovable;
    }
}

public class NodeFactory : INodeFactory
{
    public static CustomNodeViewModel CreateViewModel(VisualNode vm, double x, double y, double width, double height)
    {
        var node = new CustomNodeViewModel
        {
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Pins = new ObservableCollection<IPin>(),
            Content = vm
        };

        return node;
    }

    public IDrawingNode CreateDrawing(string name)
    {
        var drawing = new DrawingNodeViewModel
        {
            X = 0,
            Y = 0,
            Width = 900,
            Height = 600,
            Nodes = new ObservableCollection<INode>(),
            Connectors = new ObservableCollection<IConnector>(),
            EnableMultiplePinConnections = true,
            EnableSnap = true,
            SnapX = 15.0,
            SnapY = 15.0,
            EnableGrid = true,
            GridCellWidth = 15.0,
            GridCellHeight = 15.0,
        };

        var entry = CreateEntry(drawing.Width / 5, drawing.Height / 3);
        var exit = CreateExit(drawing.Width / 5 * 4, drawing.Height / 3);

        entry.Parent = drawing;
        exit.Parent = drawing;

        drawing.Nodes.Add(entry);
        drawing.Nodes.Add(exit);

        return drawing;
    }

    public INode CreateEntry(double x, double y, double width = 60, double height = 60, double pinSize = 15)
    {
        var node = CreateNode(new EntryNode(), x, y, width, height, pinSize);
        node.IsRemovable = false;

        return node;
    }

    public INode CreateExit(double x, double y, double width = 60, double height = 60, double pinSize = 15)
    {
        var node = CreateNode(new ExitNode(), x, y, width, height, pinSize);
        node.IsRemovable = false;

        return node;
    }

    public CustomNodeViewModel CreateNode(VisualNode visualNode, double x, double y, double width = 60,
        double height = 60,
        double pinSize = 15)
    {
        var pins = visualNode.GetType().GetProperties()
            .Where(_ => _.GetCustomAttribute<PinAttribute>() != null)
            .Select(prop => (prop.GetCustomAttribute<PinAttribute>(), prop)).ToArray();

        var leftPins = pins.Where(_ => _.Item1.Alignment == PinAlignment.Left).ToArray();
        var rightPins = pins.Where(_ => _.Item1.Alignment == PinAlignment.Right).ToArray();
        var topPins = pins.Where(_ => _.Item1.Alignment == PinAlignment.Top).ToArray();
        var bottomPins = pins.Where(_ => _.Item1.Alignment == PinAlignment.Bottom).ToArray();

        var maxPinTopBottom = Math.Max(topPins.Length, bottomPins.Length);
        var maxPinLeftRight = Math.Max(leftPins.Length, rightPins.Length);

        // recalculate bounds with margins to fit for pins
        width = Math.Max(width, maxPinTopBottom * (2 + pinSize) * 1.6);
        height = Math.Max(height, maxPinLeftRight * (2 + pinSize) * 1.6);

        var viewModel = CreateViewModel(visualNode, x, y, width, height);

        AddPins(pinSize, topPins, viewModel, (i) => (CalculateSinglePin(width, topPins, i), 0));
        AddPins(pinSize, bottomPins, viewModel, (i) => (CalculateSinglePin(width, bottomPins, i), height));

        AddPins(pinSize, leftPins, viewModel, (i) => (0, CalculateSinglePin(height, leftPins, i)));
        AddPins(pinSize, rightPins, viewModel, (i) => (width, CalculateSinglePin(height, rightPins, i)));

        Control nodeView = new DefaultNodeView();

        var nodeViewAttribute = visualNode.GetType().GetCustomAttribute<NodeViewAttribute>();
        if (nodeViewAttribute != null)
        {
            nodeView = (Control)Activator.CreateInstance(nodeViewAttribute.Type);
        }

        nodeView.DataContext = viewModel.Content;

        viewModel.Content = nodeView;
        viewModel.Name = visualNode.Label;

        return viewModel;
    }

    private static double CalculateSinglePin(double width, (PinAttribute, PropertyInfo prop)[] topPins, int i)
    {
        return width / (topPins.Length + 1) * (i + 1);
    }

    private static void AddPins(double pinSize, (PinAttribute, PropertyInfo prop)[] pins,
        CustomNodeViewModel viewModel, Func<int, (double, double)> positionMapper)
    {
        for (int i = 0; i < pins.Length; i++)
        {
            var pin = pins[i];

            (double baseX, double baseY) = positionMapper(i);

            viewModel.AddPin(baseX, baseY, pinSize, pinSize, pin.Item1.Alignment, pin.Item1.Name ?? pin.prop.Name);
        }
    }

    class NodeTemplate : INodeTemplate
    {
        public NodeTemplate(string title, INode template, INode preview)
        {
            Title = title;
            Template = template;
            Preview = preview;
        }

        public string Title { get; set; }
        public INode Template { get; set; }
        public INode Preview { get; set; }
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

        var normalNodes = nodes.Where(_ => !typeof(INodeFactory).IsAssignableFrom(_))
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
                    new NodeCategory {Name = category, Templates = new ObservableCollection<INodeTemplate>()});
            }

            templates.Add(new NodeTemplateViewModel
            {
                Title = node.Label, Template = CreateNode(node, 0, 0), Preview = CreateNode(node, 0, 0)
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

        return templates;
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
