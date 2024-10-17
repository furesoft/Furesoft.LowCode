using Avalonia.Controls.ApplicationLifetimes;
using Furesoft.LowCode.Designer.ViewModels;

namespace Furesoft.LowCode;

public class App : Application
{
    public static bool EnableInputOutput { get; set; } = true;

    public static bool EnableMainMenu { get; set; } = true;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        AppContext.SetData("DesignerMode", true);

        var vm = new MainViewViewModel();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow {DataContext = vm};

            DataContext = vm;
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewLifetime)
        {
            singleViewLifetime.MainView = new MainView {DataContext = vm};

            DataContext = vm;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
