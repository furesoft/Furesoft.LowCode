using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Furesoft.LowCode.Nodes.RPA.Web.Core;

public class BrowserFactory
{
    public static BrowserFactory Instance = new();
    
    private Dictionary<string, IWebDriver> _drivers { get; set; } = new Dictionary<string, IWebDriver>();

    /// <returns>the driverId</returns>
    public async Task<string> OpenAsync(string driverType, object? options = default,
        CancellationToken cancellationToken = default)
    {
        var id = Guid.NewGuid().ToString();
        IWebDriver driver;

        switch (driverType)
        {
            case DriverType.Chrome:
                var chromeDriverInstaller = new ChromeDriverInstaller();
                var chromeVersion = await chromeDriverInstaller.GetChromeVersion();
                await chromeDriverInstaller.Install(chromeVersion);
                //chromeOptions.AddArguments("headless");
                driver = new ChromeDriver(options as ChromeOptions);
                break;
            default:
                throw new Exception($"Invalid driver type {driverType}");
        }

        _drivers[id] = driver;

        return id;
    }

    public IWebDriver GetDriver(string driverId)
    {
        return _drivers[driverId];
    }

    public Task CloseBrowserAsync(string? driverId)
    {
        GetDriver(driverId!).Dispose();
        _drivers.Remove(driverId!);
        
        return Task.CompletedTask;
    }
}
