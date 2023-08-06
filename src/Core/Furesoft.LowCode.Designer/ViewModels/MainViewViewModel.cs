using System.Windows.Input;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Input;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.Core.Events;
using Furesoft.LowCode.Designer.Layout.Models;
using Furesoft.LowCode.Designer.Layout.ViewModels;
using Furesoft.LowCode.Designer.Layout.ViewModels.Documents;

namespace Furesoft.LowCode.Designer.ViewModels;

public partial class MainViewViewModel : ViewModelBase
{
    private readonly IFactory _dockFactory;

    private CancellationTokenSource _cancellationTokenSource = new();
    private IRootDock _layout;
    [ObservableProperty] private DocumentViewModel _selectedDocument;

    [ObservableProperty] private EmptyNode _selectedNode;

    public MainViewViewModel()
    {
        var nodeFactory = new NodeFactory();
        _dockFactory = new DockFactory(nodeFactory);
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
    }

    public Evaluator Evaluator { get; set; }


    public IRootDock Layout
    {
        get => _layout;
        set => SetProperty(ref _layout, value);
    }

    public ICommand NewLayout { get; }

    private void DockFactoryOnFocusedDockableChanged(object sender, FocusedDockableChangedEventArgs e)
    {
        if (e.Dockable is DocumentViewModel document)
        {
            SelectedDocument = document;
            SelectedNode = null;

            //ToDo: Optimize
            _selectedDocument.Editor.Drawing.SelectionChanged += DrawingOnSelectionChanged;
        }
    }


    [RelayCommand]
    public void Cancel()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = new();
    }


    private void DrawingOnSelectionChanged(object sender, EventArgs e)
    {
        var selectedNodes = _selectedDocument.Editor.Drawing.GetSelectedNodes()?.OfType<CustomNodeViewModel>();

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
    public Task Evaluate()
    {
        Evaluator = new(_selectedDocument.Editor.Drawing);
        await Evaluator.Execute(_cancellationTokenSource.Token);
    }

    [RelayCommand]
    public async Task Debug()
    {
        Evaluator = new(_selectedDocument.Editor.Drawing);
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
        if (_selectedDocument.Editor?.Factory is not null)
        {
            _selectedDocument.Editor.Drawing = _selectedDocument.Editor.Factory.CreateDrawing();
            _selectedDocument.Editor.Drawing.SetSerializer(_selectedDocument.Editor.Serializer);
            Evaluator = new(_selectedDocument.Editor.Drawing);
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
        if (_selectedDocument.Editor?.Serializer is null)
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
                var drawing = _selectedDocument.Editor.Serializer.Deserialize<DrawingNodeViewModel>(json);
                if (drawing is not null)
                {
                    _selectedDocument.Editor.Drawing = drawing;
                    _selectedDocument.Editor.Drawing.SetSerializer(_selectedDocument.Editor.Serializer);
                    _selectedDocument.Editor.Drawing.SelectionChanged += DrawingOnSelectionChanged;
                }
            }
            catch (Exception)
            {
            }
        }
    }

    [RelayCommand]
    private async Task Save()
    {
        if (_selectedDocument.Editor?.Serializer is null)
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
            SuggestedFileName = Path.GetFileNameWithoutExtension("graph"),
            DefaultExtension = "json",
            ShowOverwritePrompt = true
        });

        if (file is not null)
        {
            try
            {
                var json = _selectedDocument.Editor.Serializer.Serialize(_selectedDocument.Editor.Drawing);
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
}
