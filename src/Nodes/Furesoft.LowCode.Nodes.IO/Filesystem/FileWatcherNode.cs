namespace Furesoft.LowCode.Nodes.IO.Filesystem;

[NodeCategory("IO/FileSystem")]
[Description("Watch for Changes on the filesystem")]
public class FileWatcherNode : InputOutputNode
{
    public FileWatcherNode() : base("File Watcher")
    {
    }

    public string Path { get; set; }
    public string Filter { get; set; }
    public bool IncludeSubDirectories { get; set; }

    [Pin("On Change", PinAlignment.Right)] public IOutputPin OnChangedPin { get; set; }

    public NotifyFilters NotifyFilter { get; set; }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        var watcher = new FileSystemWatcher();
        watcher.Path = Path;
        watcher.Filter = Filter;
        watcher.IncludeSubdirectories = IncludeSubDirectories;
        watcher.EnableRaisingEvents = true;
        watcher.NotifyFilter = NotifyFilter;

        watcher.Created += async (sender, args) =>
        {
            await ContinueAfterEvent(cancellationToken, args);
        };
        watcher.Changed += async (sender, args) =>
        {
            await ContinueAfterEvent(cancellationToken, args);
        };
        watcher.Renamed += async (sender, args) =>
        {
            await ContinueAfterEvent(cancellationToken, args);
        };
        watcher.Deleted += async (sender, args) =>
        {
            await ContinueAfterEvent(cancellationToken, args);
        };

        await ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }

    private async Task ContinueAfterEvent(CancellationToken cancellationToken, FileSystemEventArgs args)
    {
        DefineConstant("changed", args);
        await ContinueWith(OnChangedPin, cancellationToken: cancellationToken);
    }
}
