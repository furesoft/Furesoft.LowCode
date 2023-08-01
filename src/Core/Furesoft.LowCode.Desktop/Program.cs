using System;
using System.Runtime.InteropServices;
using Avalonia;

namespace Furesoft.LowCode.Desktop;

internal static class Program
{
    static Program()
    {
        App.EnableInputOutput = true;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("BROWSER")))
        {
            App.EnableMainMenu = true;
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            App.EnableMainMenu = true;
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            App.EnableMainMenu = false;
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            App.EnableMainMenu = false;
        }
        else
        {
            App.EnableMainMenu = true;
        }
    }

    [STAThread]
    public static void Main(string[] args)
    {
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseSkia();
    }
}
