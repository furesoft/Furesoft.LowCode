using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Evaluation;
using Furesoft.LowCode.Nodes.RPA.Web.Core;
using PuppeteerSharp;

namespace Furesoft.LowCode.Nodes.RPA.Web;

public class OpenBrowserNode : WebNode
{
    public OpenBrowserNode() : base("Open Browser")
    {
    }

    [DataMember(EmitDefaultValue = false)]
    [Required]
    public Evaluatable<string> URL { get; set; }

    [DataMember(EmitDefaultValue = false)] public bool UseHeadless { get; set; }

    protected override async Task Invoke(CancellationToken cancellationToken)
    {
        using var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);

        var browser = await Puppeteer.LaunchAsync(new() {Headless = UseHeadless});

        var page = (await browser.PagesAsync())[0];
        await page.GoToAsync(Evaluate(URL));

        DefineConstant(PageVariableName, page);
        DefineConstant(BrowserVariableName, page);
    }
}
