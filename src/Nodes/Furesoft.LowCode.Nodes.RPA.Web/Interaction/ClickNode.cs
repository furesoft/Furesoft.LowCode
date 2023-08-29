using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Nodes.RPA.Web.Core;
using Furesoft.LowCode.NodeViews;

namespace Furesoft.LowCode.Nodes.RPA.Web.Interaction;

[NodeView(typeof(IconNodeView),
    "M9.5 11 15.88 16.37 15 16.55 14.36 16.67C13.73 16.8 13.37 17.5 13.65 18.07L13.92 18.65 15.28 21.59 13.86 22.25 12.5 19.32 12.24 18.74C11.97 18.15 11.22 17.97 10.72 18.38L10.21 18.78 9.5 19.35V11M8.76 8.69A.76.76 0 008 9.45V20.9C8 21.32 8.34 21.66 8.76 21.66 8.95 21.66 9.11 21.6 9.24 21.5L11.15 19.95 12.81 23.57C12.94 23.84 13.21 24 13.5 24 13.61 24 13.72 24 13.83 23.92L16.59 22.64C16.97 22.46 17.15 22 16.95 21.63L15.28 18 17.69 17.55C17.85 17.5 18 17.43 18.12 17.29 18.39 16.97 18.35 16.5 18 16.21L9.26 8.86 9.25 8.87C9.12 8.76 8.95 8.69 8.76 8.69M13 10V8H18V10H13M11.83 4.76 14.66 1.93 16.07 3.34 13.24 6.17 11.83 4.76M8 0H10V5H8V0M1.93 14.66 4.76 11.83 6.17 13.24 3.34 16.07 1.93 14.66M1.93 3.34 3.34 1.93 6.17 4.76 4.76 6.17 1.93 3.34M5 10H0V8H5V10")]
public class ClickNode : WebNode
{
    public ClickNode() : base("Click")
    {
    }

    [DataMember(EmitDefaultValue = false)]
    [Required]
    public string Selector { get; set; } = string.Empty;

    protected override async Task Invoke(CancellationToken cancellationToken)
    {
        var page = GetPage();

        var element = await page.QuerySelectorAsync(Selector);

        await element.ClickAsync();
    }
}
