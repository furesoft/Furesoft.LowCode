using Furesoft.LowCode.Evaluation;
using NiL.JS.Core;
using NiL.JS.Core.Interop;
using PuppeteerSharp;

namespace Furesoft.LowCode.Nodes.RPA.Web;

public class ScriptInitializer : IScriptModuleInitalizer
{
    public void InitEngine(Context context)
    {
        context.DefineVariable("RPA").Assign(JSObject.CreateObject());

        context.ImportAsSubObject<ScriptInitializer>("RPA", "Browser");
    }

    [JavaScriptName("openBrowser")]
    public static Result OpenBrowser(string url, bool useHeadless)
    {
        var browserFetcher = new BrowserFetcher();
        browserFetcher.DownloadAsync();

        var browser = Puppeteer.LaunchAsync(new() { Headless = useHeadless }).GetAwaiter().GetResult();

        var page = browser.PagesAsync().GetAwaiter().GetResult()[0];
        page.GoToAsync(url).GetAwaiter().GetResult();

        return Result.Ok(page);
    }
}
