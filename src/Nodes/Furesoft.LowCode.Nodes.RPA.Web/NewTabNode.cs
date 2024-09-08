using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Evaluation;
using Furesoft.LowCode.Nodes.RPA.Web.Core;

namespace Furesoft.LowCode.Nodes.RPA.Web;

[NodeIcon(
    "M9 9V7h2v2h2v2h-2v2H9v-2H7v-2h2zM0 2c0-1.1.9-2 2-2h16a2 2 0 012 2v14a2 2 0 01-2 2H2a2 2 0 01-2-2V2zm2 2v12h16V4H2z")]
public class NewTabNode() : WebNode("New Tab")
{
    [DataMember(EmitDefaultValue = false)]
    [Required]
    public Evaluatable<string> URL { get; set; }

    protected override async Task Invoke(CancellationToken cancellationToken)
    {
        var page = await GetBrowser().NewPageAsync();
        await page.GoToAsync(URL);

        DefineConstant(PageVariableName, page);
    }
}
