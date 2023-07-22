using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Furesoft.LowCode.Core;
using Furesoft.LowCode.Services;
using Furesoft.LowCode.ViewModels;
using Furesoft.LowCode.Views;
using NodeEditor.Model;
using NodeEditor.Mvvm;

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
        var vm = new MainViewViewModel {IsToolboxVisible = true};

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
