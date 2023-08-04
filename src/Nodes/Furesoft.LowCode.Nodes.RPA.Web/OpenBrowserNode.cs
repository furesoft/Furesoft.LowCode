using PuppeteerSharp;

namespace Furesoft.LowCode.Nodes.RPA.Web;

public class OpenBrowserNode : WebNode
{
    public string URL { get; set; }

    public bool UseHeadless { get; set; }
    
    public OpenBrowserNode() : base("Open Browser")
    {

    }

    protected override async Task Invoke(CancellationToken cancellationToken)
    {
        using var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
        var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = UseHeadless
        });
        
        var page = await browser.NewPageAsync();
        await page.GoToAsync(Evaluate<string>(URL));
        
        DefineConstant(pageVariableName, page);
    }
}
