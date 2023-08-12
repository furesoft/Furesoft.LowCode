using Dock.Avalonia.Controls;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.Mvvm;
using Dock.Model.Mvvm.Controls;
using Furesoft.LowCode.Designer.Layout.Models.Tools;
using Furesoft.LowCode.Designer.Layout.ViewModels.Documents;
using Furesoft.LowCode.Designer.Layout.ViewModels.Tools;

namespace Furesoft.LowCode.Designer.Layout.ViewModels;

public class DockFactory : Factory
{
    private readonly NodeFactory _nodeFactory;
    private DocumentDock _documentDock;
    private IRootDock _rootDock;

    public DockFactory(NodeFactory nodeFactory)
    {
        _nodeFactory = nodeFactory;
    }

    public override IDocumentDock CreateDocumentDock()
    {
        return new DocumentDock();
    }

    public void CreateDocument(Document document)
    {
        AddDockable(_documentDock, document);
        SetActiveDockable(document);
        SetFocusedDockable(_documentDock, document);
    }

    public override IRootDock CreateLayout()
    {
        var document1 = new GraphDocumentViewModel(_nodeFactory, "Main Graph");
        var document2 = new SourceDocumentViewModel(new("main.js", "function hello(){}"));

        var toolboxTool = new ToolboxToolViewModel(_nodeFactory) {Id = "Toolbox", Title = "Toolbox"};
        var propertiesTool = new PropertiesToolViewModel {Id = "Properties", Title = "Properties"};
        var consoleTool = new ConsoleToolViewModel {Id = "Console", Title = "Console"};
        var debugOutputTool = new DebugOutputToolViewModel {Id = "DebugOutput", Title = "Debug Output"};
        var errorTool = new ErrorsToolViewModel {Id = "Errors", Title = "Errors"};
        var projectTool = new ProjectToolViewModel {Id = "Project", Title = "Project"};

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
                    VisibleDockables = CreateList<IDockable>(toolboxTool, projectTool),
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
                new ToolDock {ActiveDockable = propertiesTool, Alignment = Alignment.Top, GripMode = GripMode.Visible}
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
                    VisibleDockables = CreateList<IDockable>(consoleTool, debugOutputTool, errorTool),
                    Alignment = Alignment.Bottom,
                    GripMode = GripMode.Visible
                }
            )
        };

        _documentDock = new()
        {
            IsCollapsable = false,
            ActiveDockable = document1,
            VisibleDockables = CreateList<IDockable>(document1, document2),
            CanCreateDocument = false
        };

        var mainLayout = new ProportionalDock
        {
            Orientation = Orientation.Horizontal,
            VisibleDockables = CreateList<IDockable>
            (
                leftDock,
                new ProportionalDockSplitter(),
                _documentDock,
                new ProportionalDockSplitter(),
                rightDock,
                new ProportionalDockSplitter(),
                bottomDock
            )
        };


        var rootDock = CreateRootDock();

        rootDock.IsCollapsable = false;
        rootDock.DefaultDockable = mainLayout;
        
        _rootDock = rootDock;

        return rootDock;
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
            ["Project"] = () => new ProjectTool()
        };

        DockableLocator = new Dictionary<string, Func<IDockable>>
        {
            ["Root"] = () => _rootDock, ["Documents"] = () => _documentDock
        };

        HostWindowLocator = new() {[nameof(IDockWindow)] = () => new HostWindow()};

        base.InitLayout(layout);
    }
}
