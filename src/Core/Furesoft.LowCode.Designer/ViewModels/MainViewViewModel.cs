using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Furesoft.LowCode.Designer.Core;
using Furesoft.LowCode.Designer.Core.Components.ViewModels;
using Furesoft.LowCode.Designer.Services;
using Furesoft.LowCode.Editor.Controls;
using Furesoft.LowCode.Editor.Model;
using Furesoft.LowCode.Editor.MVVM;

namespace Furesoft.LowCode.Designer.ViewModels;

public partial class MainViewViewModel : ViewModelBase
{
    private readonly Dictionary<string, List<INodeTemplate>> _categorizedNodeTemplates = new();

    private CancellationTokenSource _cancellationTokenSource = new();
    [ObservableProperty] private EditorViewModel _editor;
    [ObservableProperty] private string _searchTerm = string.Empty;
    [ObservableProperty] private EmptyNode _selectedNode;

    public MainViewViewModel()
    {
        _editor = new();

        var dn = new DynamicNode("Dynamic");
        dn.AddPin("Flow Input", PinAlignment.Top, PinMode.Input);

        var nodeFactory = new NodeFactory();
        nodeFactory.AddDynamicNode(dn);

        _editor.Serializer = new NodeSerializer(typeof(ObservableCollection<>));
        _editor.Factory = nodeFactory;
        _editor.Templates = _editor.Factory.CreateTemplates();
        _editor.Drawing = _editor.Factory.CreateDrawing();
        _editor.Drawing.SetSerializer(_editor.Serializer);
        _editor.Drawing.SelectionChanged += DrawingOnSelectionChanged;

        CategorizeTemplates(_editor.Templates);
        TransformToTree();

        PropertyChanged += OnPropertyChanged;
    }

    public Evaluator Evaluator { get; set; }
    public ObservableCollection<object> Templates { get; set; } = new();

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SearchTerm))
        {
            Search(SearchTerm);
        }
    }

    private void CategorizeTemplates(IList<INodeTemplate> templates)
    {
        foreach (var nodeTemplate in templates.Select(_ => (_, (CustomNodeViewModel)_.Template)))
        {
            var category = nodeTemplate.Item2.DefiningNode.GetAttribute<NodeCategoryAttribute>()?.Category ?? "General";

            if (!_categorizedNodeTemplates.ContainsKey(category))
            {
                _categorizedNodeTemplates.Add(category, new());
            }

            _categorizedNodeTemplates[category].Add(nodeTemplate._);
        }
    }

    [RelayCommand]
    public void Cancel()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = new();
    }

    [RelayCommand]
    public void Search(string term)
    {
        if (string.IsNullOrEmpty(term))
        {
            CollapseAll();
            return;
        }

        Search(Templates);
    }

    private void CollapseAll()
    {
        foreach (var template in Templates)
        {
            switch (template)
            {
                case TreeViewItem tvi:
                    tvi.IsExpanded = false;
                    tvi.IsVisible = true;
                    break;

                case NodeTemplateViewModel vm:
                    vm.IsVisible = true;
                    break;
            }
        }
    }

    private void Search(IEnumerable<object> children)
    {
        foreach (var item in children)
        {
            switch (item)
            {
                case NodeTemplateViewModel vm:
                    vm.IsVisible = string.IsNullOrEmpty(SearchTerm) ||
                                   vm.Title.IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0;
                    break;
                case TreeViewItem tvi:
                    Search(tvi.Items);

                    tvi.IsVisible = tvi.Items.OfType<NodeTemplateViewModel>().Any(child => child.IsVisible);
                    tvi.IsExpanded = tvi.IsVisible;
                    break;
            }
        }
    }

    private void TransformToTree()
    {
        var treeCache = new Dictionary<string, TreeViewItem>();
        var currentPathBuilder = new StringBuilder();

        foreach (var nodeTemplate in _categorizedNodeTemplates)
        {
            var spl = nodeTemplate.Key.Split("/");
            TreeViewItem parentItem = null;
            currentPathBuilder.Clear();

            for (var index = 0; index < spl.Length; index++)
            {
                var s = spl[index];
                currentPathBuilder.Append(index == 0 ? "" : "/").Append(s);
                var currentPath = currentPathBuilder.ToString();

                if (!treeCache.TryGetValue(currentPath, out var treeViewItem))
                {
                    treeViewItem = new() {Header = s};
                    if (index == 0)
                    {
                        Templates.Insert(0, treeViewItem);
                    }
                    else
                    {
                        treeCache[
                                currentPathBuilder.Remove(currentPath.LastIndexOf('/'),
                                    currentPath.Length - currentPath.LastIndexOf('/')).ToString()].Items
                            .Insert(0, treeViewItem);
                    }

                    treeCache.Add(currentPath, treeViewItem);
                }

                parentItem = treeViewItem;
            }

            parentItem.Header = spl[^1];

            foreach (var node in nodeTemplate.Value)
            {
                parentItem.Items.Add(node);
            }
        }
    }

    private void DrawingOnSelectionChanged(object sender, EventArgs e)
    {
        var selectedNodes = _editor.Drawing.GetSelectedNodes()?.OfType<CustomNodeViewModel>();

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
        Evaluator = new(_editor.Drawing);
        await Evaluator.Execute(_cancellationTokenSource.Token);
    }

    [RelayCommand]
    public async Task Debug()
    {
        Evaluator = new(_editor.Drawing);
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
    private void Exit()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
        {
            desktopLifetime.Shutdown();
        }
    }

    [RelayCommand]
    private void About()
    {
        // TODO: About dialog.
    }

    [RelayCommand]
    private void New()
    {
        if (Editor?.Factory is not null)
        {
            Editor.Drawing = Editor.Factory.CreateDrawing();
            Editor.Drawing.SetSerializer(Editor.Serializer);
            Evaluator = new(_editor.Drawing);
        }
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
        if (Editor?.Serializer is null)
        {
            return;
        }

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
                var drawing = Editor.Serializer.Deserialize<DrawingNodeViewModel>(json);
                if (drawing is not null)
                {
                    Editor.Drawing = drawing;
                    Editor.Drawing.SetSerializer(Editor.Serializer);
                }
            }
            catch (Exception)
            {
                //Debug.WriteLine(ex.Message);
                //Debug.WriteLine(ex.StackTrace);
            }
        }
    }

    [RelayCommand]
    private async Task Save()
    {
        if (Editor?.Serializer is null)
        {
            return;
        }

        var storageProvider = StorageService.GetStorageProvider();
        if (storageProvider is null)
        {
            return;
        }

        var file = await storageProvider.SaveFilePickerAsync(new()
        {
            Title = "Save drawing",
            FileTypeChoices = GetSaveFileTypes(),
            SuggestedFileName = Path.GetFileNameWithoutExtension("drawing"),
            DefaultExtension = "json",
            ShowOverwritePrompt = true
        });

        if (file is not null)
        {
            try
            {
                var json = Editor.Serializer.Serialize(Editor.Drawing);
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

    [RelayCommand]
    public async Task Export()
    {
        if (Editor?.Drawing is null)
        {
            return;
        }

        var storageProvider = StorageService.GetStorageProvider();
        if (storageProvider is null)
        {
            return;
        }

        var file = await storageProvider.SaveFilePickerAsync(new()
        {
            Title = "Export drawing",
            FileTypeChoices = GetExportFileTypes(),
            SuggestedFileName = Path.GetFileNameWithoutExtension("drawing"),
            DefaultExtension = "png",
            ShowOverwritePrompt = true
        });

        if (file is not null)
        {
            try
            {
                var control = new DrawingNode {DataContext = Editor.Drawing};

                var root = new ExportRoot
                {
                    Width = Editor.Drawing.Width, Height = Editor.Drawing.Height, Child = control
                };

                root.ApplyTemplate();
                root.InvalidateMeasure();
                root.InvalidateArrange();
                root.UpdateLayout();

                var size = new Size(Editor.Drawing.Width, Editor.Drawing.Height);

                if (file.Name.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    using var ms = new MemoryStream();
                    ExportRenderer.RenderPng(root, size, ms);
                    await using var stream = await file.OpenWriteAsync();
                    ms.Position = 0;
                    await stream.WriteAsync(ms.ToArray());
                }

                if (file.Name.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
                {
                    using var ms = new MemoryStream();
                    ExportRenderer.RenderSvg(root, size, ms);
                    await using var stream = await file.OpenWriteAsync();
                    ms.Position = 0;
                    await stream.WriteAsync(ms.ToArray());
                }

                if (file.Name.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    using var ms = new MemoryStream();
                    ExportRenderer.RenderPdf(root, size, ms, 96);
                    await using var stream = await file.OpenWriteAsync();
                    ms.Position = 0;
                    await stream.WriteAsync(ms.ToArray());
                }

                if (file.Name.EndsWith("xps", StringComparison.OrdinalIgnoreCase))
                {
                    using var ms = new MemoryStream();
                    ExportRenderer.RenderXps(control, size, ms, 96);
                    await using var stream = await file.OpenWriteAsync();
                    ms.Position = 0;
                    await stream.WriteAsync(ms.ToArray());
                }

                if (file.Name.EndsWith("skp", StringComparison.OrdinalIgnoreCase))
                {
                    using var ms = new MemoryStream();
                    ExportRenderer.RenderSkp(control, size, ms);
                    await using var stream = await file.OpenWriteAsync();
                    ms.Position = 0;
                    await stream.WriteAsync(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
