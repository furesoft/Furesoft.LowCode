namespace Furesoft.LowCode.Designer.ViewModels;

public partial class PromptViewModel : ObservableObject
{
    [ObservableProperty] private string _hint;
    [ObservableProperty] private string _input;
    [ObservableProperty] private string _title;
}
