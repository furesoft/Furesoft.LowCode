﻿using Dock.Avalonia.Controls;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.Mvvm;
using Dock.Model.Mvvm.Controls;
using Furesoft.LowCode.Designer.Layout.Models.Tools;
using Furesoft.LowCode.Designer.Layout.ViewModels.Tools;

namespace Furesoft.LowCode.Designer.Layout.ViewModels;

public class DockFactory(NodeFactory nodeFactory) : Factory
{
    private IRootDock _rootDock;

    public DocumentDock DocumentDock { get; private set; }

    public override IDocumentDock CreateDocumentDock()
    {
        return new DocumentDock();
    }

    public void CreateDocument(Document document)
    {
        AddDockable(DocumentDock, document);
        SetActiveDockable(document);
        SetFocusedDockable(DocumentDock, document);
    }

    public override IRootDock CreateLayout()
    {
        var toolboxTool = new ToolboxToolViewModel(nodeFactory);
        var propertiesTool = new PropertiesToolViewModel();
        var errorTool = new ErrorsToolViewModel();
        var projectTool = new ProjectToolViewModel();
        var parametersTool = new ParametersToolViewModel();

        var leftDock = new ProportionalDock
        {
            Proportion = 0.25,
            Orientation = Orientation.Horizontal,
            ActiveDockable = null,
            VisibleDockables = CreateList<IDockable>
            (
                new ToolDock
                {
                    ActiveDockable = toolboxTool,
                    VisibleDockables = CreateList<IDockable>(toolboxTool, projectTool),
                    Alignment = Alignment.Left
                }
            )
        };

        var rightDock = new ProportionalDock
        {
            Proportion = 0.25,
            Orientation = Orientation.Horizontal,
            ActiveDockable = propertiesTool,
            VisibleDockables = CreateList<IDockable>
            (
                new ToolDock
                {
                    ActiveDockable = propertiesTool,
                    VisibleDockables = CreateList<IDockable>(propertiesTool, errorTool, parametersTool),
                    Alignment = Alignment.Right,
                    GripMode = GripMode.Visible
                }
            )
        };

        DocumentDock = new()
        {
            IsCollapsable = false,
            ActiveDockable = null,
            VisibleDockables = CreateList<IDockable>(),
            CanCreateDocument = false
        };

        var mainLayout = new ProportionalDock
        {
            Id = "Home",
            Orientation = Orientation.Horizontal,
            VisibleDockables = CreateList<IDockable>
            (
                leftDock,
                new ProportionalDockSplitter(),
                DocumentDock,
                new ProportionalDockSplitter(),
                rightDock
            )
        };

        var debugView = CreateDebugView(propertiesTool);

        var rootDock = CreateRootDock();

        rootDock.IsCollapsable = false;
        rootDock.ActiveDockable = debugView;
        rootDock.DefaultDockable = mainLayout;
        rootDock.VisibleDockables = CreateList<IDockable>(debugView, mainLayout);


        _rootDock = rootDock;

        return rootDock;
    }

    private ProportionalDock CreateDebugView(PropertiesToolViewModel propertiesTool)
    {
        var consoleTool = new ConsoleToolViewModel();
        var debugOutputTool = new DebugOutputToolViewModel();
        var debugLocalsTool = new DebugToolViewModel();

        var rightDebugDock = new ProportionalDock
        {
            Proportion = 0.25,
            Orientation = Orientation.Horizontal,
            ActiveDockable = propertiesTool,
            VisibleDockables = CreateList<IDockable>
            (
                new ToolDock
                {
                    ActiveDockable = consoleTool,
                    VisibleDockables = CreateList<IDockable>(consoleTool, debugOutputTool),
                    Alignment = Alignment.Bottom,
                    GripMode = GripMode.Visible
                }
            )
        };

        var leftDebugDock = new ProportionalDock
        {
            Proportion = 0.25,
            Orientation = Orientation.Horizontal,
            ActiveDockable = null,
            VisibleDockables = CreateList<IDockable>
            (
                new ToolDock
                {
                    ActiveDockable = null,
                    VisibleDockables = CreateList<IDockable>(debugLocalsTool),
                    Alignment = Alignment.Bottom
                }
            )
        };

        var debugView = new ProportionalDock
        {
            Id = "Debug",
            Title = "Debug",
            Orientation = Orientation.Horizontal,
            VisibleDockables = CreateList<IDockable>
            (
                leftDebugDock,
                new ProportionalDockSplitter(),
                DocumentDock,
                new ProportionalDockSplitter(),
                rightDebugDock
            )
        };

        return debugView;
    }

    public override void InitLayout(IDockable layout)
    {
        ContextLocator = new()
        {
            ["Properties"] = () => new PropertiesTool(),
            ["Console"] = () => new ConsoleTool(),
            ["DebugOutput"] = () => new DebugOutputTool(),
            ["Toolbox"] = () => new ToolBoxTool(),
            ["Errors"] = () => new ErrorTool(),
            ["Project"] = () => new ProjectTool(),
            ["Parameters"] = () => new ParametersTool(),
        };

        DockableLocator = new Dictionary<string, Func<IDockable>>
        {
            ["Root"] = () => _rootDock, ["Documents"] = () => DocumentDock
        };

        HostWindowLocator = new() {[nameof(IDockWindow)] = () => new HostWindow()};

        base.InitLayout(layout);
    }
}
