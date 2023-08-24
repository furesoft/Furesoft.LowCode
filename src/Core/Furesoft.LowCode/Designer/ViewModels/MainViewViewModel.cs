using System.Windows.Input;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using Avalonia.PropertyGrid.Services;
using CommunityToolkit.Mvvm.Input;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.Core.Events;
using Dock.Model.Mvvm.Controls;
using Furesoft.LowCode.Analyzing;
using Furesoft.LowCode.Designer.Layout.ViewModels;
using Furesoft.LowCode.Designer.Layout.ViewModels.Documents;
using Furesoft.LowCode.Evaluation;
using Furesoft.LowCode.Nodes;
using Furesoft.LowCode.Nodes.Data.DataTable;
using Furesoft.LowCode.Nodes.Data.DataTable.Core;
using Furesoft.LowCode.ProjectSystem;
using NiL.JS.Core;

namespace Furesoft.LowCode.Designer.ViewModels;

public partial class MainViewViewModel : ViewModelBase
{
    private readonly DockFactory _dockFactory;

    private readonly GraphAnalyzer _graphAnalyzer = new();

    private CancellationTokenSource _cancellationTokenSource = new();

    [ObservableProperty] private ObservableCollection<Message> _errors;
    private IRootDock _layout;
    [ObservableProperty] private Project _openedProject;
    [ObservableProperty] private Document _selectedDocument;

    [ObservableProperty] private EmptyNode _selectedNode;
    [ObservableProperty] private string _text;

    public MainViewViewModel()
    {
        var nodeFactory = new NodeFactory();
        _dockFactory = new(nodeFactory);
        _dockFactory.FocusedDockableChanged += DockFactoryOnFocusedDockableChanged;

        Layout = _dockFactory?.CreateLayout();
        if (Layout is not null)
        {
            _dockFactory?.InitLayout(Layout);
            if (Layout is { } root)
            {
                root.Navigate.Execute("Home");
            }
        }

        NewLayout = new RelayCommand(ResetLayout);

        SetInitialSelectedDocument();

        OpenedProject = Project.Load("test.zip");

        CellEditFactoryService.Default.AddFactory(new EvaluatableCellEditFactory());
        CellEditFactoryService.Default.AddFactory(new DataTableColumnsCellEditFactory());
        CellEditFactoryService.Default.AddFactory(new DataTableRowsCellEditFactory());
    }

    public Evaluator Evaluator { get; set; }
    public ObservableCollection<TreeViewItem> Items { get; set; } = new();


    public IRootDock Layout
    {
        get => _layout;
        set => SetProperty(ref _layout, value);
    }

    public ICommand NewLayout { get; }

    [RelayCommand]
    public void DebugEvaluate()
    {
        var context = Evaluator.Debugger.CurrentNode.Context ?? Evaluator.Context;

        var result = context.Eval(Text);
        var item = ConvertToTreeItem(result);
        item.Header = "$result = " + item.Header;

        Items.Clear();
        Items.Add(item);

        foreach (var key in context)
        {
            Items.Add(ConvertToTreeItem(context.GetVariable(key)));
        }

        var parent = new TreeViewItem {Header = "Global Context"};
        parent.Items.Add(ConvertContextToTreeItem(context.GlobalContext));
        Items.Add(parent);
    }

    private TreeViewItem ConvertContextToTreeItem(Context c)
    {
        var root = new TreeViewItem();
        foreach (var key in c)
        {
            var parent = new TreeViewItem {Header = "Parent Context"};
            parent.Items.Add(ConvertToTreeItem(c.GetVariable(key)));

            root.Items.Add(ConvertToTreeItem(c.GetVariable(key)));
        }

        return root;
    }

    private TreeViewItem ConvertToTreeItem(JSValue value)
    {
        var item = new TreeViewItem {Header = value};
        if (value.ValueType == JSValueType.Object)
        {
            var children = ConvertJsObjectToTreeItem(value);

            foreach (var child in children)
            {
                item.Items.Add(child);
            }
        }

        return item;
    }

    private IEnumerable<TreeViewItem> ConvertJsObjectToTreeItem(JSValue value)
    {
        foreach (var kv in value)
        {
            var item = new TreeViewItem();

            item.Header = kv.Key + ": " + kv.Value;

            if (kv.Value.ValueType == JSValueType.Object)
            {
                foreach (var child in kv.Value)
                {
                    item.Items.Add(ConvertJsObjectToTreeItem(child.Value));
                }
            }

            yield return item;
        }
    }

    private void SetInitialSelectedDocument()
    {
        SelectedDocument = _dockFactory.DocumentDock.ActiveDockable as GraphDocumentViewModel;
    }

    private void DockFactoryOnFocusedDockableChanged(object sender, FocusedDockableChangedEventArgs e)
    {
        if (e.Dockable is GraphDocumentViewModel graphDocument)
        {
            SelectedNode = null;

            //ToDo: Optimize
            graphDocument.Editor.Drawing.SelectionChanged += DrawingOnSelectionChanged;
        }

        if (e.Dockable is Document document)
        {
            SelectedDocument = document;
        }
    }

    [RelayCommand]
    public void Cancel()
    {
        Layout.Navigate.Execute("Home");

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = new();
    }

    private void DrawingOnSelectionChanged(object sender, EventArgs e)
    {
        var selectedNodes = ((GraphDocumentViewModel)SelectedDocument).Editor.Drawing.GetSelectedNodes()
            ?.OfType<CustomNodeViewModel>();

        if (selectedNodes != null)
        {
            SelectedNode = selectedNodes.FirstOrDefault().DefiningNode;
        }
        else
        {
            SelectedNode = new EntryNode();
            SelectedNode = null;
        }
    }

    [RelayCommand]
    public async Task Evaluate()
    {
        Evaluator = new(((GraphDocumentViewModel)SelectedDocument).Editor.Drawing);

        await Evaluator.Execute(_cancellationTokenSource.Token);
    }

    [RelayCommand]
    public async Task Debug()
    {
        Layout.Navigate.Execute("Debug");

        Evaluator = new(((GraphDocumentViewModel)SelectedDocument).Editor.Drawing);
        Evaluator.Debugger.IsAttached = true;

        await Evaluator.Execute(_cancellationTokenSource.Token);
    }

    [RelayCommand]
    public async Task Step()
    {
        await Evaluator.Debugger.Step();
    }

    [RelayCommand]
    public async Task Continue()
    {
        await Evaluator.Debugger.Continue();
    }

    [RelayCommand]
    public void Analyze()
    {
        Errors = new(_graphAnalyzer.Analyze(((GraphDocumentViewModel)SelectedDocument).Editor.Drawing));
    }

    [RelayCommand]
    private void Exit()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
        {
            desktopLifetime.Shutdown();
        }
    }

    public void CloseLayout()
    {
        if (Layout is IDock dock)
        {
            if (dock.Close.CanExecute(null))
            {
                dock.Close.Execute(null);
            }
        }
    }

    public void ResetLayout()
    {
        if (Layout is not null)
        {
            if (Layout.Close.CanExecute(null))
            {
                Layout.Close.Execute(null);
            }
        }

        var layout = _dockFactory?.CreateLayout();
        if (layout is not null)
        {
            Layout = layout;
            _dockFactory?.InitLayout(layout);
        }
    }

    [RelayCommand]
    private void New()
    {
        var editor = ((GraphDocumentViewModel)SelectedDocument).Editor;

        editor.Drawing = editor.Factory.CreateDrawing();
        editor.Drawing.SetSerializer(editor.Serializer);
        Evaluator = new(editor.Drawing);
    }

    private List<FilePickerFileType> GetOpenFileTypes()
    {
        return new() {StorageService.Json, StorageService.All};
    }

    private static List<FilePickerFileType> GetSaveFileTypes()
    {
        return new() {StorageService.Json, StorageService.All};
    }

    private static List<FilePickerFileType> GetExportFileTypes()
    {
        return new()
        {
            StorageService.ImagePng,
            StorageService.ImageSvg,
            StorageService.Pdf,
            StorageService.Xps,
            StorageService.ImageSkp,
            StorageService.All
        };
    }

    [RelayCommand]
    private async Task Open()
    {
        var editor = ((GraphDocumentViewModel)SelectedDocument).Editor;

        var storageProvider = StorageService.GetStorageProvider();
        if (storageProvider is null)
        {
            return;
        }

        var result = await storageProvider.OpenFilePickerAsync(new()
        {
            Title = "Open drawing", FileTypeFilter = GetOpenFileTypes(), AllowMultiple = false
        });

        var file = result.FirstOrDefault();

        if (file is not null)
        {
            try
            {
                await using var stream = await file.OpenReadAsync();
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();

                var drawing = editor.Serializer.Deserialize<DrawingNodeViewModel>(json);

                if (drawing is null)
                {
                    return;
                }

                editor.Drawing = drawing;
                editor.Drawing.SetSerializer(editor.Serializer);
                editor.Drawing.SelectionChanged += DrawingOnSelectionChanged;
            }
            catch (Exception)
            {
            }
        }
    }

    [RelayCommand]
    private async Task Save()
    {
        var storageProvider = StorageService.GetStorageProvider();
        if (storageProvider is null)
        {
            return;
        }

        var file = await storageProvider.SaveFilePickerAsync(new()
        {
            Title = "Save drawing",
            FileTypeChoices = GetSaveFileTypes(),
            SuggestedFileName = Path.GetFileNameWithoutExtension("graph"),
            DefaultExtension = "json",
            ShowOverwritePrompt = true
        });

        if (file is not null)
        {
            try
            {
                var json = ((GraphDocumentViewModel)SelectedDocument).Editor.Serializer.Serialize(
                    ((GraphDocumentViewModel)SelectedDocument).Editor.Drawing);

                await using var stream = await file.OpenWriteAsync();
                await using var writer = new StreamWriter(stream);
                await writer.WriteAsync(json);
            }
            catch (Exception)
            {
                //Debug.WriteLine(ex.Message);
                //Debug.WriteLine(ex.StackTrace);
            }
        }
    }

    protected override void OnLoad()
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
        {
            lifetime.Exit += (sender, args) =>
            {
                CloseLayout();
            };
        }
    }
}
