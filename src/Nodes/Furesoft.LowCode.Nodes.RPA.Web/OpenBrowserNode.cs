using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Evaluation;
using Furesoft.LowCode.Nodes.RPA.Web.Core;
using Furesoft.LowCode.NodeViews;
using PuppeteerSharp;

namespace Furesoft.LowCode.Nodes.RPA.Web;

[NodeView(typeof(IconNodeView),
    "M0 2c0-1.1.9-2 2-2h16a2 2 0 012 2v14a2 2 0 01-2 2H2a2 2 0 01-2-2V2zm2 2v12h16V4H2zm8 3 4 5H6l4-5z")]
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
        await browserFetcher.DownloadAsync();

        var browser = await Puppeteer.LaunchAsync(new() {Headless = UseHeadless});

        var page = (await browser.PagesAsync())[0];
        await page.GoToAsync(URL);

        DefineConstant(PageVariableName, page);
        DefineConstant(BrowserVariableName, page);
    }
}
