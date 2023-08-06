using Dock.Avalonia.Controls;
using Dock.Model.Controls;
using Dock.Model.Core;
using Furesoft.LowCode.Designer.Layout.Models.Documents;
using Furesoft.LowCode.Designer.Layout.Models.Tools;
using Furesoft.LowCode.Designer.Layout.ViewModels.Docks;
using Furesoft.LowCode.Designer.Layout.ViewModels.Documents;
using Furesoft.LowCode.Designer.Layout.ViewModels.Tools;
using Factory = Dock.Model.Mvvm.Factory;
using ProportionalDock = Dock.Model.Mvvm.Controls.ProportionalDock;
using ProportionalDockSplitter = Dock.Model.Mvvm.Controls.ProportionalDockSplitter;
using ToolDock = Dock.Model.Mvvm.Controls.ToolDock;

namespace Furesoft.LowCode.Designer.Layout.ViewModels;

public class DockFactory : Factory
{
    private readonly NodeFactory _nodeFactory;
    private IRootDock _rootDock;
    private IDocumentDock _documentDock;

    public DockFactory(NodeFactory nodeFactory)
    {
        _nodeFactory = nodeFactory;
    }

    public override IDocumentDock CreateDocumentDock() => new GraphDocumentDock(_nodeFactory);

    public override IRootDock CreateLayout()
    {
        var document1 = new DocumentViewModel(_nodeFactory) {Id = "Document1", Title = "New Graph"};
        var toolboxTool = new ToolboxToolViewModel(_nodeFactory) {Id = "Toolbox", Title = "Toolbox"};
        var propertiesTool = new PropertiesToolViewModel {Id = "Properties", Title = "Properties"};
        var consoleTool = new ConsoleToolViewModel {Id = "Console", Title = "Console"};
        var debugOutputTool = new DebugOutputToolViewModel() {Id = "DebugOutput", Title = "Debug Output"};

        var leftDock = new ProportionalDock
        {
            Proportion = 0.25,
            Orientation = Orientation.Vertical,
            ActiveDockable = null,
            VisibleDockables = CreateList<IDockable>
            (
                new ToolDock
                {
                    ActiveDockable = toolboxTool,
                    VisibleDockables = CreateList<IDockable>(toolboxTool),
                    Alignment = Alignment.Bottom
                }
            )
        };

        var rightDock = new ProportionalDock
        {
            Proportion = 0.25,
            Orientation = Orientation.Vertical,
            ActiveDockable = null,
            VisibleDockables = CreateList<IDockable>
            (
                new ToolDock
                {
                    ActiveDockable = propertiesTool,
                    Alignment = Alignment.Top,
                    GripMode = GripMode.Visible
                }
            )
        };
        
        var bottomDock = new ProportionalDock
        {
            Proportion = 0.25,
            Orientation = Orientation.Horizontal,
            ActiveDockable = null,
            VisibleDockables = CreateList<IDockable>
            (
                new ToolDock
                {
                    ActiveDockable = consoleTool,
                    Alignment = Alignment.Bottom,
                    GripMode = GripMode.Visible
                },
                new ToolDock
                {
                    ActiveDockable = debugOutputTool,
                    Alignment = Alignment.Bottom,
                    GripMode = GripMode.Visible
                }
            )
        };

        var documentDock = new GraphDocumentDock(_nodeFactory)
        {
            IsCollapsable = false,
            ActiveDockable = document1,
            VisibleDockables = CreateList<IDockable>(document1),
            CanCreateDocument = true
        };

        var mainLayout = new ProportionalDock
        {
            Orientation = Orientation.Horizontal,
            VisibleDockables = CreateList<IDockable>
            (
                leftDock,
                new ProportionalDockSplitter(),
                documentDock,
                new ProportionalDockSplitter(),
                rightDock,
                new ProportionalDockSplitter(),
                bottomDock
            )
        };
        

        var rootDock = CreateRootDock();

        rootDock.IsCollapsable = false; 
        rootDock.DefaultDockable = mainLayout;

        _documentDock = documentDock;
        _rootDock = rootDock;
            
        return rootDock;
    }

    public override void InitLayout(IDockable layout)
    {
        ContextLocator = new()
        {
            ["Document1"] = () => new GraphDocument(),
            ["Properties"] = () => new PropertiesTool(),
            ["Console"] = () => new ConsoleTool(),
            ["DebugOutput"] = () => new DebugOutputTool(),
            ["Toolbox"] = () => new ToolBoxTool(),
        };

        DockableLocator = new Dictionary<string, Func<IDockable>>()
        {
            ["Root"] = () => _rootDock,
            ["Documents"] = () => _documentDock
        };

        HostWindowLocator = new()
        {
            [nameof(IDockWindow)] = () => new HostWindow()
        };

        base.InitLayout(layout);
    }
}
