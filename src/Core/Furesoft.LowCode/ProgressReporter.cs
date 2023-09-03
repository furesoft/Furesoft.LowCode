namespace Furesoft.LowCode;

public partial class ProgressReporter : ObservableObject
{
    [ObservableProperty] private string _message;
    private int _progress;

    public int Progress
    {
        get => _progress;
        set
        {
            SetProperty(ref _progress, value);
            ProgressReported();
        }
    }

    protected virtual void ProgressReported() { }
}
